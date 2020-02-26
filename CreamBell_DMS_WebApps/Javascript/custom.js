
                                    //----Customized Javascript function----Date 7 April 2016//



function ValidateLoginPage() {      //--Rahul--//
    var user = window.document.getElementById("txtUserName").value;
    var paswd = window.document.getElementById("txtPassword").value;
   
    if (user == '') {
        alert("Please Enter User Name.");
        window.document.getElementById("txtUserName").focus();
        return false;
        alert('a');
    }

    if (paswd == '') {
        alert("Please Enter Password.");
        window.document.getElementById("txtPassword").focus();
        return false;
    }
}
   

function onlyNumbers(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}



