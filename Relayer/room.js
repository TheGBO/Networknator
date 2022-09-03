class Room {
    clients = [];
    roomID = "";
    host = "";

    constructor(roomID, host) {
        this.roomID = roomID;
        this.host = host;
    }

    addClient(client) {
        if (this.clients.includes(client)) return;
        this.clients.push(client);
    }
}

module.exports = Room;