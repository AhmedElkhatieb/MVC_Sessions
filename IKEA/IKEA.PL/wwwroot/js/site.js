// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var searchBtn = document.getElementById("searchInp");
function searchEmployees() {
    debugger
    var searchTerm = $('#searchInp').val();
    $.ajax({
        url: 'https://localhost:7260/Employees/Index',
        type: 'GET',
        data: { searchTerm: searchTerm },
        success: function (data) {
            $('#employeeList').html(data);
        },
        error: function () {
            alert("Error Retrieving Data");
        }
    });
}
searchBtn.addEventListener("keyup", function () {
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status == 200) {
            document.getElementById('employeeList').innerHTML = xhr.responseText;
        }
    };
    xhr.open('GET', 'https://localhost:7260/Employees/Index');
    xhr.send();
})