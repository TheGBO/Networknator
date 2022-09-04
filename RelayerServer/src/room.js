var rooms = [];

function generateJoinCode(length) {
    chars = "1234567890abcdefghijklmnopqrstuvwxyz";
    result = "";
    for (let i = 0; i < length; i++) {
        result += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return result;
}

function createRoom (publicRoom, hostAddress){
    let room = {
        JoinCode: generateJoinCode(10),
        PublicRoom: publicRoom,
        HostAddress: hostAddress,
        clients: [],
    };
    rooms.push(room);
    return room;
}

module.exports = { createRoom, rooms }