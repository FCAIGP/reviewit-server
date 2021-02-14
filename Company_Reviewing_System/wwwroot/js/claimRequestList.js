var dataTable;
var f;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    f = true;
    // to do important
    // remove the alert message and figure out why there is a warning
    $.fn.dataTable.ext.errMode = 'none';
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/ClaimRequest/ViewPendingRequests/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "description", "width": "20%" },
            { "data": "title", "width": "20%" },
            { "data": "identificationCard", "width": "20%" },
            { "data": "proofOfWork", "width": "20%" },
            { "data": "linkedInAccount", "width": "20%" },
            {
                "data": "claimStatus",
                "render": function (data) {
                    if (data == 0) {
                        f = false;
                        return `<p>Accepted</p>`;
                    }
                    else if (data == 1) {
                        f = false;
                        return `<p>Rejected</p>`;
                    }
                    else {
                        f = true;
                        return `<p>Pending</p>`
                    }
                }, "width":"40%"
            },
            {
                "data": "claimRequestId",
                "render": function (data) {
                    if (f == true) {
                        f = false;
                        return `<div class="text-center">
                        <a onclick=accept('/ClaimRequest/Accept?id='+'${data}') class='btn btn-success text-white' style='cursor:pointer; width:80px;'>
                            ACCEPT
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:80px;'
                            onclick=reject('/ClaimRequest/Reject?id='+'${data}')>
                            REJECT
                        </a>
                        </div>`;
                    }
                    else {

                    }
                    }, "width": "40%"
                    
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}


function accept(url) {
    swal({
        title: "Are you sure you want to accept?",
        buttons: true,
        dangerMode: false
    }).then((willAccept) => {
        if (willAccept) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}



function reject(url) {
    swal({
        title: "Are you sure you want to reject?",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willReject) => {
        if (willReject) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}