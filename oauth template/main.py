from flask import Flask, redirect, url_for, session, request, render_template
from requests_oauthlib import OAuth2Session
from flask_talisman import Talisman
import os
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()

app = Flask(__name__)
app.secret_key = os.urandom(24)

app.config.update(
    SESSION_COOKIE_SECURE=True,  # Only send cookies over HTTPS
    SESSION_COOKIE_HTTPONLY=True,  # Prevent JavaScript access to cookies
    SESSION_COOKIE_SAMESITE='Strict'  # Helps mitigate CSRF
)

# Initialize Flask-Talisman to enforce HTTPS
Talisman(app, force_https=True)

# Load sensitive information from environment variables
CLIENT_ID = os.getenv('CLIENT_ID')
CLIENT_SECRET = os.getenv('CLIENT_SECRET')
REDIRECT_URI = os.getenv('REDIRECT_URI')
AUTHORIZATION_BASE_URL = 'https://accounts.google.com/o/oauth2/auth'
TOKEN_URL = 'https://oauth2.googleapis.com/token'
SCOPE = ['https://www.googleapis.com/auth/userinfo.profile', 'https://www.googleapis.com/auth/userinfo.email']
USER_INFO_URL = 'https://people.googleapis.com/v1/people/me?personFields=names,emailAddresses'

@app.route('/')
def index():
    return render_template('login.html')

@app.route('/login')
def login():
    google = OAuth2Session(CLIENT_ID, redirect_uri=REDIRECT_URI, scope=SCOPE)
    authorization_url, state = google.authorization_url(AUTHORIZATION_BASE_URL, access_type='offline', prompt='select_account')

    # Save the state in the session for later validation
    session['oauth_state'] = state
    return redirect(authorization_url)

@app.route('/callback')
def callback():
    # Check if 'oauth_state' is in the session
    if 'oauth_state' not in session:
        return redirect(url_for('login'))  # Redirect to login if state is missing

    # Recreate the OAuth2 session with the saved state
    google = OAuth2Session(CLIENT_ID, state=session['oauth_state'], redirect_uri=REDIRECT_URI)

    try:
        # Fetch the token using the authorization response
        token = google.fetch_token(
            TOKEN_URL,
            client_secret=CLIENT_SECRET,
            authorization_response=request.url
        )
        session['oauth_token'] = token  # Save the token in the session

        #raise Exception('OAuth2 token error')

    except Exception as e:
        #return f'Error fetching token: {str(e)}'
        return render_template('error.html')

    try:
        # Use the token to fetch user information
        google = OAuth2Session(CLIENT_ID, token=session['oauth_token'])
        user_info = google.get(USER_INFO_URL).json()

        # Extract user details
        email = user_info.get('emailAddresses', [{}])[0].get('value', 'No email found')
        name = user_info.get('names', [{}])[0].get('displayName', 'No name found')

        # Store user information in the session (optional)
        session['user_info'] = {
            'name': name,
            'email': email
        }

    except Exception as e:
        #return f'Error fetching user info: {str(e)}'
        return render_template('error.html')

    # Redirect to the profile page after successful login
    return redirect(url_for('profile'))

@app.route('/profile')
def profile():
    # Check if the user is logged in
    if 'oauth_token' not in session:
        return redirect(url_for('login'))  # Redirect to login if not logged in

    # Retrieve user information from the session
    user_info = session.get('user_info', {})
    name = user_info.get('name', 'No name found')
    email = user_info.get('email', 'No email found')

    return render_template('profile.html', name=name, email=email)

@app.route('/logout')
def logout():
    # Clear the session
    session.clear()
    return redirect(url_for('index'))  # Redirect to the index page

if __name__ == '__main__':
    # Use self-signed certificates for local testing
    app.run(debug=True, host= '0.0.0.0', ssl_context=('selfsigned.crt', 'selfsigned.key'))