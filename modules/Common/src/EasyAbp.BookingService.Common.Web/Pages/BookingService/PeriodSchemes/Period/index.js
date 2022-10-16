$(function () {

    var l = abp.localization.getResource('EasyAbpBookingService');

    var service = easyAbp.bookingService.periodSchemes.periodScheme;
    var createModal = new abp.ModalManager(abp.appPath + 'BookingService/PeriodSchemes/Period/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'BookingService/PeriodSchemes/Period/EditModal');

    var dataTable = $('#PeriodTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: function (requestData, callback, settings) {
            if (callback) {
                service.get(periodSchemeId).then(function (result) {
                    callback({
                        recordsTotal: result.periods.length,
                        recordsFiltered: result.periods.length,
                        data: result.periods
                    });
                });
            }
        },
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.PeriodScheme.Update'),
                                action: function (data) {
                                    editModal.open({id: data.record.id, periodSchemeId: periodSchemeId});
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.PeriodScheme.Update'),
                                confirmMessage: function (data) {
                                    return l('PeriodDeletionConfirmationMessage', data.record.id);
                                },
                                action: function (data) {
                                    service.deletePeriod(periodSchemeId, data.record.id)
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
                title: l('PeriodStartingTime'),
                data: "startingTime"
            },
            {
                title: l('PeriodDuration'),
                data: "duration"
            },
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewPeriodButton').click(function (e) {
        e.preventDefault();
        createModal.open({periodSchemeId: periodSchemeId});
    });
});
