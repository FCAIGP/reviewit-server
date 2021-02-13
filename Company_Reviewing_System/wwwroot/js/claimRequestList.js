var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
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
                "data": "claimRequestId",
                "status" : "claimStatus",
                "render": function (data,status) {
                    if (status == 1) {
                        return `<div class="text-center">
                                <p>Approved</p>
                                </div>`;
                    }
                    else if (status == 2) {
                        return `<div class="text-center">
                                <p>Rejected</p>
                                </div>`;
                    }
                    else {
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