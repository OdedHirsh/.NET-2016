var board = [];
//0 - Empty, 1 - X, 2 - O
var isXturn;
var moveCount;

resetGame();



function resetGame() {
    for (var i = 0; i < 9; i++)
        board[i] = 0;
    isXturn = true;
    moveCount = 0;
}

//cell should be number between 1 to 9
//return 1 - Valid move
//       2 - Invalid move
//       3 - Victory
//       4 - Draw
function makeMove(cell) {
    if (cell < 1 || cell > 9)
        return 2;
    cell--;
    if (board[cell] == 0) {
        moveCount++;
        board[cell] = isXturn ? 1 : 2;
        isXturn = !isXturn;
        if (moveCount >= 5 && checkVictory(cell))
            return 3;
        if (moveCount == 9)
            return 4;
        return 1;
    }
    return 2;
}
function checkVictory(cell) {
    var row = Math.floor(cell / 3);
    var column = cell % 3;
    if (board[column] == board[column + 3] &&
        board[column] == board[column + 6])
        return true;
    row *= 3;
    if (board[row] == board[row + 1] && board[row] == board[row + 2])
        return true;
    if (cell % 2 == 0) {
        var diagonal1 = board[0] != 0 && board[0] == board[4] &&
            board[4] == board[8];
        if (diagonal1)
            return true;
        var diagonal2 = board[2] != 0 && board[2] == board[4] &&
            board[4] == board[6];
        if (diagonal2)
            return true;
    }
    return false;

}
