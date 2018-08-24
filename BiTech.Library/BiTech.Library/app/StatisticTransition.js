$('#slide-two').on('click', function () {
    if ($('#slide-two1').prop('checked'))
        $('#slide-two1').prop('')
    if ($(this).prop('checked'))
        $('#disableYear').css('display', 'block')
    else
        $('#disableYear').css('display', 'none')
});


$('#slide-two1').on('click', function () {
    if ($(this).prop('checked'))
        $('#disableMonth').css('display', 'block')
    else
        $('#disableMonth').css('display', 'none')
});


function openCity(cityName, elmnt, color) {
    // Hide all elements with class="tabcontent" by default */
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Remove the background color of all tablinks/buttons
    tablinks = document.getElementsByClassName("tablink");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].style.backgroundColor = "";
    }

    // Show the specific tab content
    document.getElementById(cityName).style.display = "block";

    // Add the specific color to the button used to open the tab content
    elmnt.style.backgroundColor = color;
}

// Get the element with id="defaultOpen" and click on it
document.getElementById("defaultOpen").click();
