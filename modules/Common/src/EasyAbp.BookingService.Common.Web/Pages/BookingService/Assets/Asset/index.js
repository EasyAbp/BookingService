$(function () {

    var l = abp.localization.getResource('EasyAbpBookingService');

    var service = easyAbp.bookingService.assets.asset;
    var createModal = new abp.ModalManager(abp.appPath + 'BookingService/Assets/Asset/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'BookingService/Assets/Asset/EditModal');

    var dataTable = $('#AssetTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                                visible: abp.auth.isGranted('EasyAbp.BookingService.Asset.Update'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.Asset.Update'),
                                confirmMessage: function (data) {
                                    return l('AssetDeletionConfirmationMessage', data.record.id);
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
                title: l('AssetName'),
                data: "name"
            },
            {
                title: l('AssetAssetDefinitionName'),
                data: "assetDefinitionName"
            },
            {
                title: l('AssetAssetCategoryId'),
                data: "assetCategoryId"
            },
            {
                title: l('AssetPeriodSchemeId'),
                data: "periodSchemeId"
            },
            {
                title: l('AssetDefaultPeriodUsable'),
                data: "defaultPeriodUsable"
            },
            {
                title: l('AssetVolume'),
                data: "volume"
            },
            {
                title: l('AssetPriority'),
                data: "priority"
            },
            {
                title: l('AssetTimeInAdvance'),
                data: "timeInAdvance"
            },
            {
                title: l('AssetDisabled'),
                data: "disabled"
            },
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewAssetButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
