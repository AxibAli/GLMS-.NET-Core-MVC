($=>{
    $("._action button").on("click",function(e){
        if(e.target.dataset.edit && !undefined){
            let key = e.target.dataset.edit;
            $("._edit_key").attr("title",key);
            $("._edit_key").text(key);
            debugger;
            let isList = Boolean(JSON.parse(e.target.dataset.islist.toLocaleLowerCase()));
            let delimeter = e.target.dataset.delimeter;
            let value = $('#value_' + key).text();
            UpdateModal(key, value, isList, delimeter);
            $('#modalBtnSave').off("click");
            $('#modalBtnSave').on("click", ()=> { callServer(key, value, isList, delimeter); });
            $(".modal").modal("show");
        }else{
            alert("provide data attribute");
        }
    });
})(jQuery);


function UpdateModal(key, value, isList, delimeter) {
    debugger;
    var modalElement = '';
    if (isList) {
        modalElement = `<textarea class="form-control" id="txt_${key}" rows="5" placeholder="Multiple item with &#34;${delimeter}&#34; separated or one item"></textarea>
                         <br />
                            <input type="text" id="txtModalSearch"/> <button onclick="searchModalList('modalList_${key}')">Search</button>
                            <br /><br />
                        <ul id="modalList_${key}">`;
        
        modalElement += getModalList(key, value, delimeter);
        modalElement += '</ul>';
    }
    else {
        modalElement = `<textarea class="form-control" id="txt_${key}" rows="5">${value}</textarea>`;
    }
    $('#setting-modal').html(modalElement);
}


function callServer(key, value, isList, delimeter) {
    debugger;
    var textValue = $(`#txt_${key}`).val();
    if (isList) {
        var joinArray;
        if (value == "") {
            joinArray = [textValue];
        }
        else {
            joinArray = [value, textValue];
        }
        
        value = joinArray.join(delimeter);

        //remove duplicate values
        var arrayValues = value.split(delimeter);
        var uniqueValues = getUniqueValues(arrayValues);
        value = uniqueValues.join(delimeter);


    } else {
        value = textValue;
    }
    Ajax(key, '', value, function () {
        if (isList) {
            var modalList = `#modalList_${key}`;
            let modalElement = getModalList(key, textValue, delimeter);
            $(modalList).append(modalElement);
            $(`#txt_${key}`).val("");   
        }
        $("#value_" + key).text(value);
    });
    $('#modalBtnSave').off("click");
    $('#modalBtnSave').on("click", () => { callServer(key, value, isList, delimeter); });
}

function getUniqueValues(arrayValues) {
    debugger;
    var uniqueNames = [];
    $.each(arrayValues, function (i, el) {
        if ($.inArray(el, uniqueNames) === -1) uniqueNames.push(el);
    });
    return uniqueNames;
}

function getModalList(key, value, delimeter) {
    debugger;
    let modalElement = "";
    if (value) {
        let values = value.split(delimeter);
        for (let i = 0; i < values.length; i++) {
            modalElement += `<li id="list_${i}" class="alert-success"><span>${values[i]}</span> <button id="modalBtnDelete" data-index="${i}" onclick="deleteItem('${key}','${delimeter}',${i})">x</button></li>`;
        }
    }
    return modalElement;
}

function deleteItem(key, delimeter, item) {
    debugger;
    let valueToremoveName = `#list_${item}`;
    let valueToRemove = $(`${valueToremoveName} span`).text();
    let valueOfKey = $("#value_" + key).text();
    let list = valueOfKey.split(delimeter);
    let index = list.indexOf(valueToRemove);
    if (index > -1) { list.splice(index, 1) };
    var value = list.join(delimeter);

    Ajax(key, '', value, function () {
        $(valueToremoveName).fadeOut();
        $(valueToremoveName).remove();
        $("#value_" + key).text(value);

        $('#modalBtnSave').off("click");
        $('#modalBtnSave').on("click", () => { callServer(key, value, true, delimeter); });
    });
    
}

function UpdateKey(key) {
    debugger;
    $(`#${key}`).attr('disabled', true);
    $(`#btnUpdate_${key}`).hide();
    var NewValue = $(`#${key}`).val();
    Ajax(key, '', NewValue, function () {
        $(`#btnEdit_${key}`).show();
    });

}

function EditKey(key) {
    debugger;
    $(`#${key}`).attr('disabled', false);
    $(`#btnUpdate_${key}`).show();
    $(`#btnEdit_${key}`).hide();

}

function Ajax(Key, Value, NewValue, callback) {
    debugger;
    $.ajax({
        type: "POST",
        url: '/Settings/ChangeSetting',
        data: { Key, Value, NewValue },
        beforeSend: function () {
            showLoading();
        },
        success: function (data) {
            if (data.Status) {
                showNotification('Property updated successfully');
            }
            else {
                showNotification('Property not updated, please see log in log file', 'danger');
            }

            if (!data.SavedInMemory) {
                showNotification('Property updated successfully, but not stored in memory', 'danger');
            }

            callback(); 
        },
        error: function () {
            alert('Error occured during processing.');
        },complete: function () {
            hideLoading();
        },
    }).done(function () {
        $("#loading").remove();
    });
}


function searchConfig() {
    debugger;
    $.each($('._card_view ._group .val'), function (index, values) {
        debugger;
        const value = $('#txtSearch').val().toLowerCase();
        var menuItemText = $(values).text();
        var menuItem = menuItemText.toLowerCase();
        if (menuItem.includes(value)) {
            $(`#child_${menuItemText}`).show();
        }
        else {
            $(`#child_${menuItemText}`).hide();
        }
    });
}


function searchModalList(ulId) {
    debugger;
    $.each($(`#${ulId} li span`), function (index, values) {
        debugger;
        const value = $('#txtModalSearch').val().toLowerCase();
        var menuItemText = $(values).text();
        var menuItem = menuItemText.toLowerCase();
        if (menuItem.includes(value)) {
            $(`#list_${index}`).show();
        }
        else {
            $(`#list_${index}`).hide();
        }
    });
}
