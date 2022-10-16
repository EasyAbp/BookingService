$(function () {

    var l = abp.localization.getResource('EasyAbpBookingService');

    var service = easyAbp.bookingService.periodSchemes.periodScheme;
    var createModal = new abp.ModalManager(abp.appPath + 'BookingService/PeriodSchemes/PeriodScheme/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'BookingService/PeriodSchemes/PeriodScheme/EditModal');

    var dataTable = $('#PeriodSchemeTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(service.getList),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('PeriodSchemePeriods'),
                                action: function (data) {
                                    document.location.href = abp.appPath + 'BookingService/PeriodSchemes/Period?PeriodSchemeId=' + data.record.id;
                                }
                            },
                            {
                                text: l('SetAsDefault'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.PeriodScheme.Update'),
                                confirmMessage: function (data) {
                                    return l('PeriodSchemeSetAsDefaultConfirmationMessage', data.record.id);
                                },
                                action: function (data) {
                                    service.setAsDefault(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('SuccessfullyEdited'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            },
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.PeriodScheme.Update'),
                                action: function (data) {
                                    editModal.open({id: data.record.id});
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.PeriodScheme.Update'),
                                confirmMessage: function (data) {
                                    return l('PeriodSchemeDeletionConfirmationMessage', data.record.id);
                                },
                                action: function (data) {
                                    service.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('SuccessfullyDeleted'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
            {
                title: l('PeriodSchemeName'),
                data: "name"
            },
            {
                title: l('PeriodSchemeIsDefault'),
                data: "isDefault"
            }
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewPeriodSchemeButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
