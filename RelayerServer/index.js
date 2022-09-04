const express = require('express');
const router = require('./src/routes');
const app = express();

const port = process.env.PORT | 8800;
const ip = "127.0.0.1";

app.use(router);
app.use(express.json())

app.listen(port, ip, () => {
    console.log(`(http) Listening on http://${ip}:${port}`);
});