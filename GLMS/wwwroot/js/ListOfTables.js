﻿
let flag = false;
let getPageData = "getTableData";
$(function () {
    
    $('#ddlTables').select2();
    $('#ddlSorting').select2();
    ScriptOnPageLoad();

    $('#ddlTables').on('change', function () {
        LoadTableData();
        createPageForTableList(Operations.onLoad);
    });
    $('#ddlSorting').on('change', function () {
        LoadTableData();
        createPageForTableList(Operations.onLoad);
    });
    $('#dpFromDate').on('change', function () {
        LoadTableData();
        createPageForTableList(Operations.onLoad);
    });
    $('#dpToDate').on('change', function () {
        LoadTableData();
        createPageForTableList(Operations.onLoad);
    });
    
});

function LoadTableData() {
    debugger;
    let tableheading = '';
    //var tablename = $("#ddlTables").find(":selected").text();
    let tablename = $('select#ddlTables').find(':selected')[0].dataset["tablename"];
    let schemaName = $('select#ddlTables').find(':selected')[0].dataset["schemaname"];
    ajaxCall('/Crud/GettableHeadings', { tablename, schemaName }, function (result) {
       
        if (result.Response) {
            $(result.Data).each(function (index, value) {                
                tableheading += `<th>${value}</th>`
            });
            tableheading += `<th>Operations</th>`;
            $("table thead tr").html();
            $("table thead tr").html(tableheading);
        } else {
            showNotification(result.Message, 'danger');
            console.error(result.Error);
        }
    }, 'GET');
}

function ScriptOnPageLoad() {

    let options = '';
    ajaxCall('/Crud/Gettables', null, function (result) {
        debugger;
        if (result.Response) {
            $(result.Data).each(function (index, value) {              
                options += `<option value="${index}" data-primaryKeyColumn=${value.ColumnName} data-SchemaName=${value.SchemaName}  data-tableName=${value.TableName}>${value.SchemaName}.${value.TableName}</option>`
            });
            $("#ddlTables").html(options);
            LoadTableData();
            //$("#ProductId").html(options);
            if (IsListPage) {
                createPageForTableList(Operations.onLoad);
            }
            if (flag) {
                setDropDownValues();
            }
            flag = true;
        } else {
            showNotification(result.Message, 'danger');
            console.error(result.Error);
        }
    }, 'GET');
}

//function call when have to create page according to pagesize
function createPageForTableList(operationCall) {
    debugger;
    let pageSize_ = $("#pageSize").val();
    let getFormData = $("#ddlTables").val();
    let fromDate = $("#dpFromDate").val();
    let ToDate = $("#dpToDate").val();
    var tablename = $("#ddlTables").find(":selected").text();
    let primaryKeyColumn = $('select#ddlTables').find(':selected')[0].dataset["primarykeycolumn"];
    let tablename1 = $('select#ddlTables').find(':selected')[0].dataset["tablename"];
    let schemaName = $('select#ddlTables').find(':selected')[0].dataset["schemaname"];
    if (!(operationCall === Operations.onSubmit && getFormData === null)) {
        debugger;
        AjaxReqForPaginationForTableList(pageSize_, 1, tablename, primaryKeyColumn, fromDate, ToDate, tablename1, schemaName, operationCall);
    }
    return false;
}

//Pagination Call on load and after search
function PaginationForTableList(totalPages_, pageSize_) {
    var firstPageClick = false;
    var tablename = $("#ddlTables").find(":selected").text();
    let fromDate = $("#dpFromDate").val();
    let ToDate = $("#dpToDate").val();
    let primaryKeyColumn = $('select#ddlTables').find(':selected')[0].dataset["primarykeycolumn"];
    let tablename1 = $('select#ddlTables').find(':selected')[0].dataset["tablename"];
    let schemaName = $('select#ddlTables').find(':selected')[0].dataset["schemaname"];
    debugger;
    CallPagination(totalPages_, function (event, page) {
        if (firstPageClick) {
            AjaxReqForPaginationForTableList(pageSize_, page, tablename, primaryKeyColumn, fromDate, ToDate, tablename1, schemaName, Operations.onPaginationComplete);
        }
        firstPageClick = true;
    })
}

//Ajax request for SLB_PropsAndPageNo to create pagination according to data
function AjaxReqForPaginationForTableList(PageSize, PageNo, formData, primaryKeyColumn, fromdate, Todate, tablename, schemaName, operationCall) {
    let sorting = $('select#ddlSorting').find(':selected')[0].dataset["sorting"];
    ajaxCall(getPageDataURL, { PageSize, PageNo, formData, primaryKeyColumn, sorting, fromdate, Todate, tablename, schemaName }, function (result) {
        debugger;
        if (!result.Response) {
            showNotification(result.Message, 'danger');
            console.error(result.Error);
        } else {
            $(".TItem-badge").html(result.ListData.TotalItems);
            $("#tBody").html(result.ListData.View);
            switch (operationCall) {
                case Operations.onSubmit:
                    //$("#searchModal").modal('toggle');
                    $("#lblSearchProps").html(
                        `<svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-filter" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                      <path fill-rule="evenodd" d="M6 10.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 0 1h-3a.5.5 0 0 1-.5-.5zm-2-3a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm-2-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5z"/>
                                    </svg>
                                    Filter by ${getObjProps(formData)}
                                    <a href="javascript:window.location.reload(false);">Reset</a>`);
                    PaginationForTableList(result.ListData.TotalPages, PageSize);
                    break;
                case Operations.onLoad:
                case Operations.onPageSizeChange:
                    PaginationForTableList(result.ListData.TotalPages, PageSize);
                    break;
                case Operations.onPaginationComplete:
                    break;
                default:
                    break;
            }
        }
    });
}

function ViewModalData(modalBodyHTML) {

    let label = 'Edit Table';

    $("#crudModalBody").html(modalBodyHTML);
    $("#submit_Btn").val(`${label}`);
    //ScriptOnPageLoad();
    $("#crudModal").modal({
        backdrop: 'static',
        keyboard: false
    });


}

function EditRecord(id) {
    let label = 'Edit Record';
    let modalBodyHTML = `
                        <h6 class="text-center">
                                                    <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-pencil" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                                        <path fill-rule="evenodd" d="M11.293 1.293a1 1 0 0 1 1.414 0l2 2a1 1 0 0 1 0 1.414l-9 9a1 1 0 0 1-.39.242l-3 1a1 1 0 0 1-1.266-1.265l1-3a1 1 0 0 1 .242-.391l9-9zM12 2l2 2-9 9-3 1 1-3 9-9z" />
                                                        <path fill-rule="evenodd" d="M12.146 6.354l-2.5-2.5.708-.708 2.5 2.5-.707.708zM3 10v.5a.5.5 0 0 0 .5.5H4v.5a.5.5 0 0 0 .5.5H5v.5a.5.5 0 0 0 .5.5H6v-1.5a.5.5 0 0 0-.5-.5H5v-.5a.5.5 0 0 0-.5-.5H3z" />
                                                    </svg>
                                                    ${label}
                       </h6> `;
    var tablename = $("#ddlTables").find(":selected").text();
    //var id = 1;
    ajaxCall('/Crud/getTableDataWithColumnType', { tablename, id }, function (result) {
        if (!result.Response) {
            showNotification(result.Message, 'danger');
            console.error(result.Error);
        }
        else {
            $(result.Data).each(function (index, value) {
                console.log(value.Item1);
                console.log(value.Item2);
                console.log(value.Item3);
                debugger;
                if (value.Item3 === "System.Int32" || value.Item3 === "System.Int64" || value.Item3 === "System.String" || value.Item3 ==="Microsoft.SqlServer.Types.SqlGeography") {

                    var disabledStr = "";
                    var inputValue = "";

                    if (value.Item2 === null) {
                        inputValue = "";
                    }
                    else
                    {
                        inputValue = `value = "${value.Item2}"`;
                    }

                    if (value.Item1.toLowerCase() === "id") {
                        disabledStr = "disabled";
                    }
                    modalBodyHTML += `                                            
                    <div class="col-6">
                        <h6 class="mt-3 fileMessage">${value.Item1}</h6>
                        <input type="text"  title="" ${inputValue}  name=${value.Item1} id=${value.Item1} data-value=${value.Item2} data-type=${value.Item3} placeholder="Enter ${value.Item1}" class="form-control inputfile edittable" ${disabledStr} />
                    </div>`;

                }
                else if (value.Item3 === "System.Boolean") {
                    debugger;
                    var checkedstr = "";
                    if (value.Item2==="True") {
                        checkedstr = "checked";
                    }

                    modalBodyHTML += `                                            
                    <div class="col-6">
                        <label class="mt-3 fileMessage">${value.Item1}
                      <input type="checkbox" class="edittable" id=${value.Item1} data-type=${value.Item3} data-value=${value.Item2} name=${value.Item1} ${checkedstr}>
                     </label>
                    </div>`;
                }
                else if (value.Item3 === "System.DateTime") {
                    debugger;
                    modalBodyHTML += `                                            
                    <div class="col-6">
                        <h6 class="mt-3 fileMessage">${value.Item1}</h6>
                        <input type="datetime-local" required title="" value=${value.Item2} name=${value.Item1} data-value=${value.Item2} data-type=${value.Item3} id=${value.Item1} placeholder="Enter ${value.Item1}" class="form-control inputfile edittable" />
                    </div>`;
                }

            });
            modalBodyHTML += `<div class="col-12 mt-3 mt-3">
                    <input class="btn btn-primary" onclick=SaveTableData(this) id="submit_Btn" type="submit"   value="Create Message" />
                    </div>`;
            ViewModalData(modalBodyHTML);
        }
    });
    return false;
}

//function ChangeStatus()
//{
//    debugger;
//    if ($("#isDeleted").is(":checked") == true) {
//        $("#isDeleted").attr("checked", "checked");
//        $("#isDeleted").checked = true;
//    }
//    else
//    {
//        $("#isDeleted").attr("checked", "checked");
//        $("#isDeleted").removeAttr("checked");
//        $("#isDeleted").checked = false;
//    }
//}

function tableData(columnName, columnValue, columnType,nullvalue) {
    this.columnName = columnName;
    this.columnValue = columnValue;
    this.columnType = columnType;
    this.nullableData = nullvalue;
}

function SaveTableData(input) {

    var array = [];
    for (i = 0; i < document.getElementsByClassName("edittable").length; i++) {
        console.log(document.getElementsByClassName("edittable")[i].value);
        console.log(document.getElementsByClassName("edittable")[i].dataset["type"]);
        var datavalue = "";

        if (document.getElementsByClassName("edittable")[i].dataset["type"] === "System.Boolean")
        {
            if (document.getElementsByClassName("edittable")[i].columnName == "IsActive")
            {
                datavalue = $("#IsActive").is(":checked");
            }
            else if (document.getElementsByClassName("edittable")[i].columnName == "isDeleted")
            {
                datavalue = $("#isDeleted").is(":checked");
            }

        }
        else
        {
            datavalue = document.getElementsByClassName("edittable")[i].value;
        }

        debugger;
        var data = new tableData(document.getElementsByClassName("edittable")[i].id, datavalue, document.getElementsByClassName("edittable")[i].dataset["type"], document.getElementsByClassName("edittable")[i].dataset["value"]);

        array.push(data);

    }

    var tablename = $("#ddlTables").find(":selected").text();

    debugger;
    console.log(array);

    //if (data.userid == "") {
    //    showNotification("Please select registered from", 'danger');
    //    return;
    //}

    //if (data.requestedFrom == "") {
    //    showNotification("Please select username", 'danger');
    //    return;
    //}

    ajaxCall('/Crud/SaveTableData', { array, tablename }, function (result) {
        debugger;
        if (!result.Response) {
            showNotification(result.Message, 'danger');
            console.error(result.Error);
        }
        else {
            $("#crudModal").modal('toggle');
            showNotification(result.Message);
            LoadTableData();
            //$("#ProductId").html(options);
            if (IsListPage) {
                createPageForTableList(Operations.onLoad);
            }
        }
    });
    return false;

}

function DeleteRecord(id)
{
    var tablename = $("#ddlTables").find(":selected").text();
    ajaxCall('/Crud/DeleteRecord', { id, tablename }, function (result) {
        debugger;
        if (!result.Response) {
            showNotification(result.Message, 'danger');
            console.error(result.Error);
        }
        else {
           
            showNotification(result.Message);
            LoadTableData();
          
            if (IsListPage) {
                createPageForTableList(Operations.onLoad);
            }
        }
    });
    return false;
}


