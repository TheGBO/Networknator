const express = require("express");
const room = require('./room.js');
const app = express();
const net = require('net');

const roomListPort = 8800;
const tcpRelayPort = 7777;

var rooms = [];

app.get('/serverList',(req, res) => {
    res.json({ serverList: rooms });
});

app.post('/', (req, res) => {
    var ip = req.socket.remoteAddress;
    var roomToAdd = new room()
    
});

const tcp = net.createServer((socket) => {
    
});

tcp.listen(tcpRelayPort, () => {
    console.log(`running tcp on ${tcpRelayPort}`);
});

app.listen(roomListPort, () => {
    console.log(`running http on ${roomListPort}`);
});