const router = require('express').Router();
const room = require('./room');

router.post('/room', (req, res) => {
    res.json(room.createRoom(true, req.socket.remoteAddress))
});

router.post('/joinroom/:JoinCode', (req, res) => {
    room.rooms.forEach(roomData => {
        console.log(roomData);

        if(!req.params.JoinCode) return res.status(404).json({});

        if (roomData.JoinCode == req.params.JoinCode) {
            if (!roomData.clients.includes(req.socket.remoteAddress)) {
                roomData.clients.push(req.socket.remoteAddress);
            }
            return res.json(roomData);
        } 
    });
    return res.status(404).json({});
});

module.exports = router;