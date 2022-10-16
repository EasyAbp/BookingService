$(function () {

    var l = abp.localization.getResource('EasyAbpBookingService');

    var service = easyAbp.bookingService.assetSchedules.assetSchedule;
    var createModal = new abp.ModalManager(abp.appPath + 'BookingService/AssetSchedules/AssetSchedule/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'BookingService/AssetSchedules/AssetSchedule/EditModal');

    var dataTable = $('#AssetScheduleTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                                text: l('Edit'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.AssetSchedule.Update'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.AssetSchedule.Update'),
                                confirmMessage: function (data) {
                                    return l('AssetScheduleDeletionConfirmationMessage', data.record.id);
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
                title: l('AssetScheduleDate'),
                data: "date"
            },
            {
                title: l('AssetScheduleAssetId'),
                data: "assetId"
            },
            {
                title: l('AssetSchedulePeriodSchemeId'),
                data: "periodSchemeId"
            },
            {
                title: l('AssetSchedulePeriodId'),
                data: "periodId"
            },
            {
                title: l('AssetSchedulePeriodUsable'),
                data: "periodUsable"
            },
            {
                title: l('AssetScheduleTimeInAdvance'),
                data: "timeInAdvance"
            },
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewAssetScheduleButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
