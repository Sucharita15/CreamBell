function IsValidDate(myDate) {
    var filter = /^([012]?\d|3[01])-([Jj][Aa][Nn]|[Ff][Ee][bB]|[Mm][Aa][Rr]|[Aa][Pp][Rr]|[Mm][Aa][Yy]|[Jj][Uu][Nn]|[Jj][u]l|[aA][Uu][gG]|[Ss][eE][pP]|[oO][Cc][Tt]|[Nn][oO][Vv]|[Dd][Ee][Cc])-(19|20)\d\d$/
    return filter.test(myDate);
}
function ValidateDate(e)
{

    //debugger;
    var isValid = IsValidDate(e.value);
    if (isValid) {
        //alert('Correct format');
    }
    else {
       
        alert('Please Enter The Date In Format: dd-MMM-yyyy');
        e.value = '';
        
    }
    return isValid
}