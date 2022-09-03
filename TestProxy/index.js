const net = require('net');

var tcpClients = []

const tcpServer = net.createServer(socket => {
    socket.on('connect', () => {
        tcpClients.push(socket);
    })

    socket.on('data', msg => {
        tcpClients.forEach(client => {
            console.log(msg.toString());
            client.write(msg);
        });
    });

    socket.on('close', hadError => {
        tcpClients.filter(item => item !== socket);
    })

    socket.on('error', err => {
        tcpClients.filter(item => item !== socket);
    });
});

tcpServer.listen(1244);