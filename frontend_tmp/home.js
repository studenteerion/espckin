// API base URL
const apiUrl = 'http://127.0.0.1:9898';

// Function to load all data
function loadAll() {
    fetch(`${apiUrl}/all`)
        .then(response => response.json())
        .then(data => {
            displayResult(data);
        })
        .catch(error => {
            console.error(error);
            alert("Error fetching data.");
        });
}

// Function to get teacher data based on plate
function getByPlate() {
    const plate = prompt("Enter Plate Number:");
    if (plate) {
        fetch(`${apiUrl}/plate/${plate}`)
            .then(response => response.json())
            .then(data => {
                displayResult(data);
            })
            .catch(error => {
                console.error(error);
                alert("Error fetching data.");
            });
    }
}

// Function to get plate data based on teacher
function getByTeacher() {
    const teacher = prompt("Enter Teacher ID:");
    if (teacher) {
        fetch(`${apiUrl}/teachers/${teacher}`)
            .then(response => response.json())
            .then(data => {
                displayResult(data);
            })
            .catch(error => {
                console.error(error);
                alert("Error fetching data.");
            });
    }
}

// Function to get teachers by entry
function getByEntry() {
    const entry = prompt("Enter Entry Zone:");
    if (entry) {
        fetch(`${apiUrl}/entry/${entry}`)
            .then(response => response.json())
            .then(data => {
                displayResult(data);
            })
            .catch(error => {
                console.error(error);
                alert("Error fetching data.");
            });
    }
}

// Function to display the result as a table
function displayResult(data) {
    const resultDiv = document.getElementById('result');
    resultDiv.innerHTML = ''; // Clear previous results

    if (Array.isArray(data) && data.length > 0) {
        const table = document.createElement('table');
        table.style.border = '1px solid black';
        
        // Create table header
        const headerRow = document.createElement('tr');
        const headers = Object.keys(data[0]);
        headers.forEach(header => {
            const th = document.createElement('th');
            th.textContent = header;
            th.style.border = '1px solid black';
            headerRow.appendChild(th);
        });
        table.appendChild(headerRow);

        // Create rows for each item in the data array
        data.forEach(item => {
            const row = document.createElement('tr');
            headers.forEach(header => {
                const td = document.createElement('td');
                td.textContent = item[header] || '';
                td.style.border = '1px solid black';
                row.appendChild(td);
            });
            table.appendChild(row);
        });

        resultDiv.appendChild(table);
    } else {
        resultDiv.innerHTML = '<p>No data available.</p>';
    }
}

// Create record
document.getElementById('create-form').addEventListener('submit', function (e) {
    e.preventDefault();

    const table = document.getElementById('create-table').value;
    const data = JSON.parse(document.getElementById('create-data').value);

    fetch(`${apiUrl}/create/${table}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(data => {
            alert("Record created successfully");
            console.log(data);
        })
        .catch(error => {
            console.error(error);
            alert("Error creating record.");
        });
});

// Update record
document.getElementById('update-form').addEventListener('submit', function (e) {
    e.preventDefault();

    const table = document.getElementById('update-table').value;
    const id = document.getElementById('update-id').value;
    const data = JSON.parse(document.getElementById('update-data').value);

    fetch(`${apiUrl}/update/${table}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(data => {
            alert("Record updated successfully");
            console.log(data);
        })
        .catch(error => {
            console.error(error);
            alert("Error updating record.");
        });
});

// Delete record
document.getElementById('delete-form').addEventListener('submit', function (e) {
    e.preventDefault();

    const table = document.getElementById('delete-table').value;
    const id = document.getElementById('delete-id').value;

    fetch(`${apiUrl}/delete/${table}/${id}`, {
        method: 'DELETE'
    })
        .then(response => response.json())
        .then(data => {
            alert("Record deleted successfully");
            console.log(data);
        })
        .catch(error => {
            console.error(error);
            alert("Error deleting record.");
        });
});
