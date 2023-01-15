$(function () {

    var l = abp.localization.getResource('EasyAbpBookingService');

    var service = easyAbp.bookingService.assetOccupancies.assetOccupancy;
    var createModal = new abp.ModalManager(abp.appPath + 'BookingService/AssetOccupancies/AssetOccupancy/CreateModal');

    var dataTable = $('#AssetOccupancyTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                                text: l('Delete'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.AssetOccupancy.Update'),
                                confirmMessage: function (data) {
                                    return l('AssetOccupancyDeletionConfirmationMessage', data.record.id);
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
                title: l('AssetOccupancyAssetId'),
                data: "assetId"
            },
            {
                title: l('AssetOccupancyAsset'),
                data: "asset"
            },
            {
                title: l('AssetOccupancyAssetDefinitionName'),
                data: "assetDefinitionName"
            },
            {
                title: l('AssetOccupancyVolume'),
                data: "volume"
            },
            {
                title: l('AssetOccupancyDate'),
                data: "date"
            },
            {
                title: l('AssetOccupancyStartingTime'),
                data: "startingTime"
            },
            {
                title: l('AssetOccupancyDuration'),
                data: "duration"
            },
            {
                title: l('AssetOccupancyOccupierUserId'),
                data: "occupierUserId"
            },
            {
                title: l('AssetOccupancyOccupierName'),
                data: "occupierName"
            },
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewAssetOccupancyButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
