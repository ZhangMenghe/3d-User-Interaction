// JavaScript

var brick : Transform;
function Start () {
    for (var y = 0; y < 5; y++) {
        for (var x = 0; x < 5; x++) {
            Instantiate(brick, Vector3 (x, y, 0), Quaternion.identity);
        }
    }
}