
function LoadApplicationDtls(appid) {

    $.ajax({
        url: getAppDtlsUrl,
        type: 'GET',
        data: { ApplicationId: appid },
        success: function (res) {
            if (res.success) {
                var rnrno = res.data.BusinessDetails.rnr_number;
                if (rnrno !== "" && rnrno !== undefined && rnrno !== null) {
                    ModernAlert.showConfirm(
                        'You have already submitted your application with ARN No. ' + rnrno + '. Do you want to track your application?',
                        'Navigate',
                        function (confirmed) {
                            if (confirmed) {
                                window.location.href = landingUrl + '?arn=' + encodeURIComponent(rnrno);
                            }
                        }
                    );
                }
                else {
                    AssignEnterPriseControls(res.data.BusinessDetails);
                    if (res.data.OwnerDetails.length > 0) {
                        AssignOwnerDtlsControls(res.data.OwnerDetails[0]);
                    }
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
                    if (res.data.DocumentDetails.length > 0) {
                        isDocData = "Y";
                        renderTable('FilesBody', res.data.DocumentDetails, BindDocTable);
                        updateSerialNumbers('DocumentsTable');
                    }
                    loaddistrict("", 'dir_prtnr_district');
                    loaddistrict("", 'addl_plc_district');
                    var step = parseInt(res.data.BusinessDetails.progress_step - 1);
                    if (step == -1) { step = 0; }
                    showTab(step);
                }
            }
        },
        error: function (xhr) {
            showError('Server error: ' + xhr.statusText);
        }
    });

}

function AssignEnterPriseControls(d) {
    $('#lblApplicationId').html("Application Id: " + d.ApplicationId);
    loadDivisions(d.division_code);
    loadCircles(d.division_code, d.circle_code);
    loaddistrict(d.district_code,'district');
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
    $('#emp_below_15000').val(d.EmpBelow_15000);
    $('#emp_between_15001_20000').val(d.EmpBetween_15001_20000);
    $('#emp_above_20000').val(d.EmpAbove_20000);
    $('#tot_emp').val(d.TotalEmployees);
}
function AssignOwnerDtlsControls(d) {
    loaddistrict(d.district, 'O_distict');
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
    /*$('#O_distict').val(d.district);*/
    $('#O_state_name').val(d.state_name);
    $('#O_country').val(d.country);
    $('#O_pincode').val(d.pincode);
}

function AssignAuthPersonControls(d, res) {
    $("input[name='rdb_auth_prsn'][value='" + res + "']").prop("checked", true);
    if (res.toLowerCase() == "yes") {
        loaddistrict(d.auth_prsn_district, 'auth_district');
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
            ? `<button class="btn btn-sm btn-primary viewFile" data-url="${r.document_path}">
                        View
                   </button>`
            : ''
        }
                <button class="btn btn-sm btn-danger deleteFile">
                    Delete
                </button>
            </td>
        </tr>
    `;
}
function loadFullSummary(AppId) {

    $.ajax({
        url: getFullSummaryUrl,
        type: 'GET',
        data: { ApplicationId: AppId },
        success: function (res) {
            if (!res.success) {
                //showError(res.message);
                return;
            }
            renderBizDtls(res.data.BusinessDetails);
            renderOwnerDtls(res.data.OwnerDetails[0]);
            renderAuthPersonDtls(res.data.AuthPersonDetails[0]);
            renderTable('rnrPartnersbody', res.data.DirectorPartners, renderPartnerRow);
            renderTable('rnrPlacesbody', res.data.AddlPlacesOfBiz, renderAddlPlaceRow);
            renderTable('rnrBankbody', res.data.BankDetails, renderBankRow);
            renderTable('rnrDocbody', res.data.DocumentDetails, renderDocRow);
        },
        error: function (xhr) {
            //showError('Server error: ' + xhr.statusText);
        }
    });
}

function renderTable(tbodyId, rows, rowFn) {
    const tbody = $('#' + tbodyId);
    tbody.empty();
    if (!rows || rows.length === 0) {
        tbody.html(`<tr><td colspan="99" class="empty-row">
                            <i class="fas fa-info-circle"></i> No records found
                        </td></tr>`);
        return;
    }
    rows.forEach((r, i) => tbody.append(rowFn(r, i + 1)));
}

function renderBizDtls(d) {
    if (!d) return;
  
    $('#rnrSpnEnterpriseName').text(d.EnterPriseName);
    $('#rnrSpnAddress').text(d.FullAddress);
    $('#rnrSpnConstitution').text(d.BusinessConstitution);
    $('#rnrSpnPAN').text(d.BusinessPan);
    $('#rnrSpnMobile').text(d.MobileNo);
    $('#rnrSpnEmail').text(d.EmailId);
    $('#rnrSpnBelow15k').text(d.EmpBelow_15000);
    $('#rnrSpnAbove15k').text(d.EmpBetween_15001_20000);
    $('#rnrSpnAbove20k').text(d.EmpAbove_20000);
    $('#rnrSpnTotalEmployees').text(d.TotalEmployees);

}

function renderOwnerDtls(d) {
    if (!d) return;
    $('#rnrSpnOwnerName').text(d.owner_name);
    $('#rnrSpnOwnerFatherName').text(d.father_name);
    $('#rnrSpnOwnerStatus').text(d.status_of_individual);
    $('#rnrSpnOwnerPAN').text(d.pan);
    $('#rnrSpnOwnerAadhar').text(d.aadhaar);
    $('#rnrSpnOwnerMobile').text(d.mobile_no);
    $('#rnrSpnOwnerEmail').text(d.email_id);
    $('#rnrSpnOwnerNomination').text("Yes");
    $('#rnrSpnOwnerAddress').text(d.FullAddress);

}
function renderAuthPersonDtls(d) {
    if (!d) return;
    $('#rnrSpnAuthPersonName').text(d.auth_prsn_name);
    $('#rnrSpnAuthFatherName').text(d.auth_prsn_father_name);
    $('#rnrSpnAuthEmail').text(d.email_id);
    $('#rnrSpnAuthPan').text(d.auth_prsn_pan);
    $('#rnrSpnAuthAadhar').text(d.auth_prsn_aadhaar);
    $('#rnrSpnAuthMobile').text(d.auth_prsn_mobile_no);
    $('#rnrSpnAuthAddress').text(d.FullAddress);

}

function renderEmpRow(r, i) {
    return `<tr>
                               <td class="row-num">${i}</td>
                               <td>${r.empName}</td>
                               <td>${r.designation}</td>
                               <td>${r.empType}</td>
                               <td>${r.gender}</td>
                               <td>${r.doj}</td>
                               <td>₹${Number(r.salary).toLocaleString('en-IN')}</td>
                               <td>${r.mobile || empty()}</td>
                           </tr>`;
}


function renderPartnerRow(r, i) {
    return `<tr>
                                <td class="row-num">${i}</td>
                                <td>${r.dir_name}</td>
                                <td>${r.type_drp}</td>
                                <td>${r.drawing_remuneration}</td>
                                <td><span class="mono-val">${r.pan || 'N/A'}</span></td>
                                <td><span class="mono-val">${r.aadhar || 'N/A'}</span></td>
                                <td>${r.email_id}</td>
                                <td>${r.mobile_no || empty()}</td>
                                <td>${r.FullAddress}</td>
                            </tr>`;
}

function renderAddlPlaceRow(r, i) {
    return `<tr>
                            <td class="row-num">${i}</td>
                            <td>${r.country}</td>
                            <td>${r.state_name}</td>
                            <td>${r.district}</td>
                            <td>${r.mandal}</td>
                            <td>${r.FullAddress}</td>
                        </tr>`;
}

function renderAuthRow(r, i) {
    const expired = isExpired(r.validTo);
    return `<tr>
                            <td class="row-num">${i}</td>
                            <td>${r.authName}</td>
                            <td>${r.designation}</td>
                            <td>${r.mobile}</td>
                            <td>${r.validFrom}</td>
                            <td>${r.validTo}</td>
                            <td><span class="badge ${expired ? 'badge-expired' : 'badge-green'}">${expired ? 'Expired' : 'Active'}</span></td>
                        </tr>`;
}




function renderPbzRow(r, i) {
    const isActive = r.status?.toLowerCase() === 'active';
    return `<tr>
                            <td class="row-num">${i}</td>
                            <td>${r.bzName}</td>
                            <td>${r.address}</td>
                            <td>${r.natureOfBz}</td>
                            <td>${r.fromDate}${r.toDate ? ' → ' + r.toDate : ''}</td>
                            <td><span class="badge ${isActive ? 'badge-green' : 'badge-secondary-acc'}">${r.status}</span></td>
                        </tr>`;
}

function renderBankRow(r, i) {
    return `<tr>
                            <td class="row-num">${i}</td>
                            <td>${r.bank_name}</td>
                            <td>${r.ifsc_code}</td>
                            <td>${r.account_number}</td>
                            <td>${r.account_holder_name}</td>
                            <td>${r.branch_address}</td>
                        </tr>`;
    /*<td><span class="badge ${r.isPrimary ? 'badge-primary-acc' : 'badge-secondary-acc'}">${r.isPrimary ? 'Primary' : 'Secondary'}</span></td>*/
}

function renderDocRow(r, i) {
    //const viewUrl = r.web_url || `/Documents/View/${r.master_doc_id}`;

    return `<tr>
                        <td class="row-num">${i}</td>
                        <td>${r.document_type}</td>
                        <td>${r.uploaded_date}</td>
                        <td>${r.document_path
        ? `<a href="${r.document_path}" target="_blank" class="btn btn-sm" onclick="return openDocument('${r.document_path}');" style="font-size: small;">
                                <i class="fas fa-eye"></i> View
                               </a>`
            : empty()}</td>
                    </tr>`;
}

function openDocument(url) {
    window.open(url, '_blank');
    return false;
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