
var myparent: Transform ;
var startMessage:Transform ;
function Start () {

}
function showStartMessage(){
    var text = Instantiate(startMessage, myparent.position, transform.rotation);
    text.transform.parent = myparent.transform;
}
function Update () {

}