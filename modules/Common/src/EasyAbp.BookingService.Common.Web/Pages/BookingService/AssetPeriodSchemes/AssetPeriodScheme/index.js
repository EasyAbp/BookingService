$(function () {

    var l = abp.localization.getResource('EasyAbpBookingService');

    var service = easyAbp.bookingService.assetPeriodSchemes.assetPeriodScheme;
    var createModal = new abp.ModalManager(abp.appPath + 'BookingService/AssetPeriodSchemes/AssetPeriodScheme/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'BookingService/AssetPeriodSchemes/AssetPeriodScheme/EditModal');

    var dataTable = $('#AssetPeriodSchemeTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                                visible: abp.auth.isGranted('EasyAbp.BookingService.AssetPeriodScheme.Update'),
                                action: function (data) {
                                    editModal.open({
                                        date: data.record.date,
                                        assetId: data.record.assetId
                                    });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.AssetPeriodScheme.Update'),
                                confirmMessage: function (data) {
                                    return l('AssetPeriodSchemeDeletionConfirmationMessage', data.record.id);
                                },
                                action: function (data) {
                                    service.delete({
                                            date: data.record.date,
                                            assetId: data.record.assetId
                                        })
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
                title: l('AssetPeriodSchemePeriodSchemeId'),
                data: "periodSchemeId"
            },
            {
                title: l('AssetPeriodSchemeAssetId'),
                data: "assetId"
            },
            {
                title: l('AssetPeriodSchemeDate'),
                data: "date"
            },
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewAssetPeriodSchemeButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
