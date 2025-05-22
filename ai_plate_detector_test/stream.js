const http = require('http');
const fs = require('fs');
const path = require('path');

// Set the port for the server
const PORT = 3000;

// Get the file path of the MP4 located in Downloads
const FILE_PATH = path.join(process.env.HOME, 'Downloads', 'input.mp4'); // Modify if the file has a different name

const server = http.createServer((req, res) => {
    const range = req.headers.range; // Get the range header from the request

    if (!range) {
        res.statusCode = 416; // Range Not Satisfiable
        res.end('Range header is required');
        return;
    }

    const stats = fs.statSync(FILE_PATH);
    const fileSize = stats.size;
    const CHUNK_SIZE = 10 * 1024 * 1024; // 10MB chunks

    const start = Number(range.replace(/\D/g, '')); // Remove non-digit characters to get start position
    const end = Math.min(start + CHUNK_SIZE, fileSize - 1);

    const readStream = fs.createReadStream(FILE_PATH, { start, end });

    // Set the correct headers for partial content
    res.writeHead(206, { // 206 indicates Partial Content
        'Content-Range': `bytes ${start}-${end}/${fileSize}`,
        'Accept-Ranges': 'bytes',
        'Content-Length': end - start + 1,
        'Content-Type': 'video/mp4'
    });

    // Pipe the read stream to the response
    readStream.pipe(res);
});

server.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});