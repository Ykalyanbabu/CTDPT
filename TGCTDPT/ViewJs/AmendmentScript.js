

function LoadApplicationDtls(_Ptin) {

    $.ajax({
        url: getAppDtlsUrl,
        type: 'GET',
        data: { ptin: _Ptin },
        success: function (res) {
            if (res.success) {
                AssignEnterPriseControls(res.data.BusinessDetails);
                if (res.data.OwnerDetails.length > 0) {
                    AssignOwnerDtlsControls(res.data.OwnerDetails[0]);
                }
                else {
                    LoadDistricts("", 'O_distict');
                }
                LoadDistricts("", 'auth_district');
                if (res.data.AuthPersonDetails.length > 0) {
                    AssignAuthPersonControls(res.data.AuthPersonDetails[0], res.data.BusinessDetails.Nominated_Auth_Person);
                }
                if (res.data.DirectorPartners.length > 0) {
                    isPartnerData = "Y";
                    renderTable('PartnerBody', res.data.DirectorPartners, BindPartnerTable);
                }
                if (res.data.AddlPlacesOfBiz.length > 0) {
                    isAddlPlaceData = "Y";
                    $("input[name='addl_rdb'][value='Yes']").prop("checked", true);
                    renderTable('AddlPlaceBody', res.data.AddlPlacesOfBiz, BindAddlPlaceTable);
                }
                else {
                    $("input[name='addl_rdb'][value='No']").prop("checked", true);
                }
                if (res.data.BankDetails.length > 0) {
                    isBankData = "Y";
                    renderTable('BankDtlsBody', res.data.BankDetails, BindBankTable);
                }
                /*if (res.data.DocumentDetails.length > 0) {
                    isDocData = "Y";
                    renderTable('FilesBody', res.data.DocumentDetails, BindDocTable);
                    updateSerialNumbers('DocumentsTable');
                }*/
                LoadDistricts("", 'dir_prtnr_district');
                LoadDistricts("", 'addl_plc_district');
            }
            else {
                ModernAlert.toast.info('Detials not found for this PTIN');
                LoadCountries();
                loaddistrictddls();
                loadDivisions();

            }
        },
        error: function (xhr) {
            showError('Server error: ' + xhr.statusText);
        }
    });
}
function IsPartnerDiv(res) {
    if (res == "Y") {
        $('#divYesPartners').show();
        $('#divNoPartners').hide();
        $('#btnAddPartner').show();
        $('#divPartnerstable').show();
        $('#hdr1').show();
    }
    else {
        $('#divYesPartners').hide();
        $('#divNoPartners').show();
        $('#btnAddPartner').hide();
        $('#divPartnerstable').hide();
        $('#hdr1').hide();
    }
}
function AssignEnterPriseControls(d) {
    $('#lblApplicationId').html("Application Id: " + d.ApplicationId);
    loadDivisions(d.division_code);
    loadCircles(d.division_code, d.circle_code);
    LoadDistricts(d.district_code, 'district');
    $('#enterprise_name').val(d.EnterPriseName);
    $('#business_pan').val(d.BusinessPan);
    $('#cobz').val(d.BusinessConstitution);
    $('#email_id').val(d.EmailId);
    $('#mobile_no').val(d.MobileNo);
    $('#door_no').val(d.DoorNo);
    $('#road_street').val(d.RoadStreet);
    $('#locality').val(d.Locality);
    $('#city').val(d.City);
    $('#mandal').val(d.Mandal);
    //$('#district').val(d.district_code);
    $('#pincode').val(d.Pincode);
    if (d.isemp == "1") {
        var res = "Yes"
        $("input[name='rdb_isemp'][value='" + res + "']").prop("checked", true);
        $('#emp_below_15000').val(d.EmpBelow_15000);
        $('#emp_between_15001_20000').val(d.EmpBetween_15001_20000);
        $('#emp_above_20000').val(d.EmpAbove_20000);
        $('#tot_emp').val(d.TotalEmployees);
        $('#divIsEmp').show();
    }
    else {
        var res = "No";
        $("input[name='rdb_isemp'][value='" + res + "']").prop("checked", true);
        $('#emp_below_15000').val("");
        $('#emp_between_15001_20000').val("");
        $('#emp_above_20000').val("");
        $('#tot_emp').val("");
        $('#divIsEmp').hide();
    }
}
function AssignOwnerDtlsControls(d) {

    $('#O_owner_name').val(d.owner_name);
    $('#O_father_name').val(d.father_name);
    $('#O_status_of_individual').val(d.status_of_individual);
    $('#O_pan').val(d.pan);
    $('#O_aadhaar').val(d.aadhaar);
    $('#O_mobile_no').val(d.mobile_no);
    $('#O_email_id').val(d.email_id);
    $('#O_door_no').val(d.door_no);
    $('#O_road_street').val(d.road_street);
    $('#O_locality').val(d.locality);
    $('#O_city').val(d.city);
    $('#O_mandal').val(d.mandal);
    $('#O_pincode').val(d.pincode);
    LoadCountries(d.country, "O_country");
    if (d.country == "1") {
        LoadStates(d.state_name, "O_state_name");
        $('#divOwnerState').show();
        $('#divOwnerDist').show();
    }
    else {
        $('#divOwnerState').hide();
        $('#divOwnerDist').hide();
    }
    if (d.state_name == "36") {
        LoadDistricts(d.district, 'O_distict');
        $('#O_distict').show();
        $('#txtOwnerDist').hide();
    }
    else {
        $('#O_distict').hide();
        $('#txtOwnerDist').show();
        $('#txtOwnerDist').val(d.district_name);
    }
}

function AssignAuthPersonControls(d, res) {
    $("input[name='rdb_auth_prsn'][value='" + res + "']").prop("checked", true);
    if (res.toLowerCase() == "yes") {
        LoadDistricts(d.auth_prsn_district, 'auth_district');
        $('#auth_name').val(d.auth_prsn_name);
        $('#auth_fname').val(d.auth_prsn_father_name);
        $('#auth_email').val(d.email_id);
        $('#auth_door_no').val(d.auth_prsn_door_no);
        $('#auth_road_street').val(d.auth_prsn_road_street);
        $('#auth_locality').val(d.auth_prsn_locality);
        $('#auth_city').val(d.auth_prsn_city);
        /*$('#auth_district').val(d.auth_prsn_district);*/
        $('#auth_pincode').val(d.auth_prsn_pincode);
        $('#auth_pan').val(d.auth_prsn_pan);
        $('#auth_aadhaar').val(d.auth_prsn_aadhaar);
        $('#auth_mobile_no').val(d.auth_prsn_mobile_no);
        $('#divAuthorised').show();
    }
    else {
        $('#divAuthorised').hide();
    }
}
function BindPartnerTable(r, i) {
    let obj = {
        dir_prtnr_name: r.dir_name,
        dir_prtnr_type: r.type_drp,
        dir_prtnr_remunrtn: r.drawing_remuneration,
        dir_prtnr_door_no: r.door_no || '',
        dir_prtnr_road_street: r.road_street || '',
        dir_prtnr_locality: r.locality || '',
        dir_prtnr_city: r.city || '',
        dir_prtnr_mandal: r.mandal || '',
        dir_prtnr_district_code: r.district || '',
        dir_prtnr_state: r.state_name || '',
        dir_prtnr_country: r.country || '',
        dir_prtnr_pincode: r.pincode || '',
        dir_prtnr_pan: r.pan || '',
        dir_prtnr_aadhaar: r.aadhaar || '',
        dir_prtnr_email: r.email_id || '',
        dir_prtnr_mobile_no: r.mobile_no || '',
        dir_prtnr_district_name: r.district_name || ''
    };
    dataListdir.push(obj);

    return `
        <tr data-item='${JSON.stringify(obj)}'>
            <td>${i}</td>
            <td>${obj.dir_prtnr_name}</td>
            <td>${obj.dir_prtnr_type}</td>
            <td>${obj.dir_prtnr_remunrtn}</td>
            <td>${obj.dir_prtnr_door_no}</td>
            <td>${obj.dir_prtnr_road_street}</td>
            <td>${obj.dir_prtnr_locality}</td>
            <td>${obj.dir_prtnr_city}</td>
            <td>${obj.dir_prtnr_mandal}</td>
            <td>${obj.dir_prtnr_district_name}</td>
            <td>${obj.dir_prtnr_state}</td>
            <td>${obj.dir_prtnr_country}</td>
            <td>${obj.dir_prtnr_pincode}</td>
            <td>${obj.dir_prtnr_pan}</td>
            <td>${obj.dir_prtnr_aadhaar}</td>
            <td>${obj.dir_prtnr_email}</td>
            <td>${obj.dir_prtnr_mobile_no}</td>
            <td class="text-center">
                <button type="button" class="btn btn-sm btn-danger deleteRow">
                    <i class="fa fa-trash"></i>
                </button>
            </td>
        </tr>
    `;

}
function BindAddlPlaceTable(r, i) {
    let obj = {
        addl_plc_country: r.country || '',
        addl_plc_state: r.state_name || '',
        addl_plc_district_code: r.district || '',
        addl_plc_mandal: r.mandal || '',
        addl_plc_door_no: r.door_no || '',
        addl_plc_road_street: r.road_street || '',
        addl_plc_locality: r.locality || '',
        addl_plc_city: r.city || '',
        addl_plc_pincode: r.pincode || '',
        is_Additional_place: r.is_Additional_place || '',
        addl_plc_district_name: r.district_name || ''
    };
    dataListadl.push(obj);
    return `
        <tr data-item='${JSON.stringify(obj)}'>
            <td>${i}</td>
            <td>${obj.addl_plc_country}</td>
            <td>${obj.addl_plc_state}</td>
            <td>${obj.addl_plc_district_name}</td>
            <td>${obj.addl_plc_mandal}</td>
            <td>${obj.addl_plc_door_no}</td>
            <td>${obj.addl_plc_road_street}</td>
            <td>${obj.addl_plc_locality}</td>
            <td>${obj.addl_plc_city}</td>
            <td>${obj.addl_plc_pincode}</td>
            <td class="text-center">
                <button type="button" class="btn btn-sm btn-warning editRow" style="display:none;">
                    <i class="fa fa-edit"></i>
                </button>
                <button type="button" onclick="onPlaceDelete(this);" class="btn btn-sm btn-danger deleteRow">
                    <i class="fa fa-trash"></i>
                </button>
            </td>
        </tr>
    `;

}
function BindBankTable(r, i) {

    let obj = {
        account_number: r.account_number,
        account_holder_name: r.account_holder_name,
        bank_id: r.bank_id,
        ifsc_code: r.ifsc_code,
        branch_address: r.branch_address,
        bank_name: r.bank_name
    };
    dataListb.push(obj);
    return `
        <tr data-item='${JSON.stringify(obj)}'>
            <td>${i}</td>
            <td>${obj.bank_name}</td>
            <td>${obj.account_number}</td>
            <td>${obj.account_holder_name}</td>
            <td>${obj.ifsc_code}</td>
            <td>${obj.branch_address}</td>
            <td class="text-center">
                <button type="button" class="btn btn-sm btn-warning editRow" style="display:none;">
                    <i class="fa fa-edit"></i>
                </button>
                <button type="button" onclick="onPlaceDelete(this);" class="btn btn-sm btn-danger deleteRow">
                    <i class="fa fa-trash"></i>
                </button>
            </td>
        </tr>
    `;
}
function BindDocTable(r, i) {

    let obj = {
        master_docid: r.master_doc_id,
        document_type: r.document_type,
        document_path: r.document_path,
        file: null
    };

    documentList.push(obj);

    let index = documentList.length - 1;
    $('#doc_name option[value="' + r.master_doc_id + '"]').prop('disabled', true);

    return `
        <tr data-index="${index}">
            <td>${i}</td>
            <td>${r.document_type}</td>
            <td class="text-center">
                ${r.document_path
            ? `<button class="btn btn-sm btn-primary viewFile same-size-btn" data-url="${r.document_path}">
                        View
                   </button>`
            : ''
        }
                
            </td>
            <td class="text-center">
                <button class="btn btn-sm btn-primary deleteFile same-size-btn" style="background: red;">
                    Delete
                </button>
            </td>
        </tr>
    `;
}

function renderTable(tbodyId, rows, rowFn) {
    const tbody = $('#' + tbodyId);
    tbody.empty();
    if (!rows || rows.length === 0) {
        tbody.html(`<tr><td colspan="99" class="empty-row">
                            <i class="fas fa-info-circle"></i> Details not Provided
                        </td></tr>`);
        return;
    }
    if (tbodyId == "rnrPlacesbody" || tbodyId == "rnrPartnersbody") {
        if (rows[0].country == null) {
            tbody.html(`<tr><td colspan="99" class="empty-row">
                            <i class="fas fa-info-circle"></i> Details not Provided
                        </td></tr>`);
            return;
        }
    }
    rows.forEach((r, i) => tbody.append(rowFn(r, i + 1)));
}
function empty() { return '<span class="empty-val">Not provided</span>'; }
function isExpired(dateStr) {
    if (!dateStr) return false;
    return new Date(dateStr.split('-').reverse().join('-')) < new Date();
}

function updateSerialNumbers(res) {
    $('#' + res + ' tbody tr').each(function (index) {
        $(this).find('td:first').text(index + 1);
    });
}


function fn_SaveAmendment() {
    var _Busniness = null; var _Emp = null; var _KeyPerson = null; var _Auth = null; var _Partners = []; var _Branches = []; var _Bank = [];
    if (CheckBuz == "Y") {
        _Busniness = {
            enterprise_name: $("#enterprise_name").val(),
            business_pan: $("#business_pan").val(),
            cobz: $("#cobz").val(),
            email_id: $("#email_id").val(),
            mobile_no: $("#mobile_no").val(),
            door_no: $("#door_no").val(),
            road_street: $("#road_street").val(),
            locality: $("#locality").val(),
            city: $("#city").val(),
            mandal: $("#mandal").val(),
            district_code: $("#district").val(),
            district_name: $('#district option:selected').text(),
            pincode: $("#pincode").val(),
            division_code: $("#bz_division").val(),
            circle_code: $("#bz_circle").val()
        };
    }
    if (CheckEmp == "Y") {
        var _isemp = "";
        if ($('#chkisEmp').is(":checked") == true) { _isemp = "1" } else { _isemp = "0" }
        _Emp = {
            emp_below_15000: $("#emp_below_15000").val() || 0,
            emp_between_15001_20000: $("#emp_between_15001_20000").val() || 0,
            emp_above_20000: $("#emp_above_20000").val() || 0,
            tot_emp: $("#tot_emp").val() || 0,
            isemp: _isemp
        };
    }
    if (CheckKeyPerson == "Y") {
        var DistirctName = "";
        if ($('#O_state_name').val() == "36") {
            DistirctName = $('#O_state_name option:selected').text();
        }
        else {
            DistirctName = $('#O_distict').val();
        }
        _KeyPerson = {
            O_owner_name: $("#O_owner_name").val(),
            O_father_name: $("#O_father_name").val(),
            O_status_of_individual: $("#O_status_of_individual").val(),
            O_email_id: $("#O_email_id").val(),
            O_door_no: $("#O_door_no").val(),
            O_road_street: $("#O_road_street").val(),
            O_locality: $("#O_locality").val(),
            O_city: $("#O_city").val(),
            O_mandal: $("#O_mandal").val(),
            O_district: $("#O_distict").val(),
            O_district_name: DistirctName,
            O_state_name: $("#O_state_name").val(),
            O_country: $("#O_country").val(),
            O_pincode: $("#O_pincode").val(),
            O_pan: $("#O_pan").val(),
            O_aadhaar: $("#O_aadhaar").val(),
            O_mobile_no: $("#O_mobile_no").val(),
            O_nomination: $("input[name='Nomination']:checked").val(),
            application_id: $("#application_id").val()
        };
    }
    if (CheckAuth == "Y") {
        _Auth = {
            auth_prsn_name: $("#auth_name").val() || null,
            auth_prsn_father_name: $("#auth_fname").val() || null,
            email_id: $("#auth_email").val() || null,
            auth_prsn_door_no: $("#auth_door_no").val() || null,
            auth_prsn_road_street: $("#auth_road_street").val() || null,
            auth_prsn_locality: $("#auth_locality").val() || null,
            auth_prsn_city: $("#auth_city").val() || null,
            auth_prsn_district: $("#auth_district").val() || null,
            auth_prsn_pincode: $("#auth_pincode").val() || null,
            auth_prsn_pan: $("#auth_pan").val() || null,
            auth_prsn_aadhaar: $("#auth_aadhaar").val() || null,
            auth_prsn_mobile_no: $("#auth_mobile_no").val() || null,
            is_Authorised_person: $("input[name='rdb_auth_prsn']:checked").val(),
        };
    }
    if (CheckPartners == "Y") {
        $("#dirprtnrTable tbody tr").each(function () {
            var item = $(this).attr("data-item");
            if (item) {
                _Partners.push(JSON.parse(item));
            }
        });
    }
    if (CheckBranches == "Y") {
        $("#AdditionalPlaceTable tbody tr").each(function () {
            var item = $(this).attr("data-item");
            if (item) {
                _Branches.push(JSON.parse(item));
            }
        });
    }
    if (CheckBank == "Y") {
        $("#bankdetailsTable tbody tr").each(function () {
            var item = $(this).attr("data-item");
            if (item) {
                _Bank.push(JSON.parse(item));
            }
        });
    }
    var formData = new FormData();

    // ✅ Prepare document metadata array
    var documents = [];
    let i = 0;

    documentList.forEach(function (item, index) {

        if (item != null) {

            if (!item.document_type) {
                alert("File missing at row " + (index + 1));
                return;
            }

            documents.push({
                application_id: "",
                master_doc_id: item.master_docid,
                document_type: item.document_type,
                document_path: ""
            });

            if (item.file) {
                formData.append("files", item.file);
            }

            i++;
        }
    });
    var requestData = {
        Business: _Busniness,
        Emp: _Emp,
        KeyPerson: _KeyPerson,
        Auth: _Auth,
        Partners: _Partners,
        Branches: _Branches,
        Bank: _Bank,
        Documents: documents
    };

    formData.append("request", JSON.stringify(requestData));

    $.ajax({
        url: SaveUrl,
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        dataType: 'JSON',
        success: function (response) {
            if (response.success) {
                ModernAlert.showAlert('Your Amendment Request is Submitted with Reference No : ' + response.data, 'You will be Redirecting Dasboard', 'Submitted',
                    {
                        autoClose: true,
                        duration: 3000,
                    }
                );
                setTimeout(() => {
                    window.location.href = HomeUrl;
                }, 2000);
                $('.modern-close').hide();
            }
            else {
                ModernAlert.toast.info('Unable to Save Data due to Session Expired or Uncaught error');
                $('#btnSubmit').attr('disabled', false);
            }
        },
        //error: function () {
        //    ModernAlert.toast.info('Unable to Save Data due to Session Expired or Uncaught error');
        //    $('#btnSubmit').attr('disabled', false);
        //}
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    });

}
function SubmitAmendment() {

    if (CheckBuz == "Y") {
        var container = document.querySelector("#divBusiness");
        var inputs = container.querySelectorAll("input, select");
        var valid = true;

        inputs.forEach(function (input) {

            if (input.offsetParent !== null) {
                if (input.value.trim() === "") {
                    input.style.border = "1px solid red";
                    valid = false;
                } else {
                    input.style.border = "";
                }
            }

        });
        if (!valid) {
            ModernAlert.toast.info('Please fill all required fields');
            return false;
        }
    }
    if (CheckEmp == "Y") {

        var isemp = $("input[name='rdb_isemp']:checked").val();
        if (!isemp) {
            ModernAlert.toast.info('Please select Yes or No for Employee Status');
            return false;
        }

        if ($('#chkisEmp').is(":checked") == true) {
            var container = document.querySelector("#divEmployee");
            var inputs = container.querySelectorAll("input");
            var valid = true;

            inputs.forEach(function (input) {

                if (input.offsetParent !== null) {

                    if (input.value.trim() === "") {
                        input.style.border = "1px solid red";
                        valid = false;
                    }
                    else {
                        input.style.border = "";
                    }
                }

            });

            if (!valid) {
                alert("Please fill all required fields.");
                return false;
            }
        }
        else {
            $("#emp_below_15000").val("");
            $("#emp_between_15001_20000").val("");
            $("#emp_above_20000").val("");
            $("#tot_emp").val("");
        }
    }
    if (CheckKeyPerson == "Y") {
        if ($('#O_owner_name').val() == "") {
            ModernAlert.toast.info('Please enter Owner Name');
            $('#O_owner_name').focus();
            return false;
        }
        if ($('#O_father_name').val() == "") {
            ModernAlert.toast.info('Please enter Owner Father Name');
            $('#O_father_name').focus();
            return false;
        }
        if ($('#O_status_of_individual').val() == "") {
            ModernAlert.toast.info('Please select Status of the Individual ');
            $('#O_status_of_individual').focus();
            return false;
        }
        if ($('#O_email_id').val() == "") {
            ModernAlert.toast.info('Please enter Owner Email Id');
            $('#O_email_id').focus();
            return false;
        }
        if ($('#O_door_no').val() == "") {
            ModernAlert.toast.info('Please enter Owner Door Number');
            $('#O_door_no').focus();
            return false;
        }
        if ($('#O_road_street').val() == "") {
            ModernAlert.toast.info('Please enter Owner Road / Street / Building Name');
            $('#O_road_street').focus();
            return false;
        }
        if ($('#O_locality').val() == "") {
            ModernAlert.toast.info('Please enter Owner Locality ');
            $('#O_locality').focus();
            return false;
        }
        if ($('#O_city').val() == "") {
            ModernAlert.toast.info('Please enter Owner City / Town / Village ');
            $('#O_city').focus();
            return false;
        }
        if ($('#O_mandal').val() == "") {
            ModernAlert.toast.info('Please enter Owner Madal');
            $('#O_mandal').focus();
            return false;
        }
        if ($('#O_country').val() == "") {
            ModernAlert.toast.info('Please select Owner Country');
            $('#O_country').focus();
            return false;
        }
        if ($('#O_country').val() == "1") {
            if ($('#O_state_name').val() == "") {
                ModernAlert.toast.info('Please select Owner State');
                $('#O_state_name').focus();
                return false;
            }

            if ($('#O_state_name').val() == "36") {
                if ($('#O_distict').val() == "") {
                    ModernAlert.toast.info('Please select Owner District');
                    $('#O_distict').focus();
                    return false;
                }
            }
            if ($('#O_state_name').val() != "36") {
                if ($('#txtOwnerDist').val() == "") {
                    ModernAlert.toast.info('Please enter Owner District');
                    $('#txtOwnerDist').focus();
                    return false;
                }
            }
        }
        if ($('#O_pincode').val() == "") {
            ModernAlert.toast.info('Please enter Owner Pincode');
            $('#O_pincode').focus();
            return false;
        }
        if ($('#O_pan').val() == "") {
            ModernAlert.toast.info('Please enter Owner PAN');
            $('#O_pan').focus();
            return false;
        }
        if ($('#O_mobile_no').val() == "" || $('#O_mobile_no').val().length > 10 || $('#O_mobile_no').val().length != 10) {
            ModernAlert.toast.info('Please enter valid Owner Mobile Number');
            $('#O_mobile_no').focus();
            return false;
        }
    }
    if (CheckAuth == "Y") {
        var nomination = $("input[name='rdb_auth_prsn']:checked").val();
        if (!nomination) {
            ModernAlert.toast.info('Please select Yes or No for Authorised Person');
            return false;
        }
        var container = document.querySelector("#divAuthPerson");
        var inputs = container.querySelectorAll("input, select");
        var valid = true;

        inputs.forEach(function (input) {

            if (input.offsetParent !== null) {
                if (input.id === "auth_aadhaar" && input.value.trim() === "") {
                    input.style.border = "";
                    return;
                }
                if (input.value.trim() === "") {
                    input.style.border = "1px solid red";
                    valid = false;
                }
                else {
                    input.style.border = "";
                }
            }

        });

        if (!valid) {
            ModernAlert.toast.info('please fill all required fields correctly.');
            return false;
        }
    }
    if (CheckPartners == "Y") {
        let rowCount = $("#PartnerBody tr[data-item]").length;
        if (rowCount === 0) {
            ModernAlert.toast.info('Please add at least one Partner/Director Details');
            return false;
        }
    }
    if (CheckBranches == "Y") {
        var is_addl_place = $("input[name='addl_rdb']:checked").val();
        console.log(is_addl_place);
        if (!is_addl_place) {
            ModernAlert.toast.info('Please select Yes or No Additional Place of Business');
            return false;
        }
        if (is_addl_place === 'Yes') {
            let rowCount = $("#AdditionalPlaceTable tr[data-item]").length;
            if (rowCount === 0) {
                ModernAlert.toast.info('Please add atleast one Additional Business Place Details');
                return false;
            }
        }
    }
    if (CheckBank == "Y") {
        let rowCount = $("#BankDtlsBody tr[data-item]").length;
        if (rowCount === 0) {
            ModernAlert.toast.info('Please add atleast one row of Bank Details.');
            return false;
        }
    }
    if ($('#FilesBody tr[data-index]').length === 0) {
        ModernAlert.toast.info('Please upload atleast one document');
        return false;
    }
    ModernAlert.showConfirm(
        'Are you sure you want to Submit?',
        'Confirm Submit',
        function (confirmed) {
            if (confirmed) {
                $('#btnSubmit').attr('disabled', true);
                fn_SaveAmendment();
            }
            else {
                ModernAlert.toast.info('Action cancelled');
                $('#btnSubmit').attr('disabled', false);
            }
        }
    );
}
function updateSerialNumbers(res) {
    $('#' + res + ' tbody tr').each(function (index) {
        $(this).find('td:first').text(index + 1);
    });
}