function keyTest(event) {
    if (event.code == "Enter" && event.ctrlKey == true)
    {
        console.log("New Message!");
        console.log(document.getElementById("sendmessage"));
        document.getElementById("sendmessage").click();
        return false;
    }
    return true;
}

function scrollIntoView() {
    if (document.querySelector("#anchor") !== null)
        document.querySelector("#anchor").scrollIntoViewIfNeeded()
}