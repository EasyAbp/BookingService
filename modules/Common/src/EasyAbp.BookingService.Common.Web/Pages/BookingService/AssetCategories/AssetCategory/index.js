$(function () {

    var l = abp.localization.getResource('EasyAbpBookingService');

    var service = easyAbp.bookingService.assetCategories.assetCategory;
    var createModal = new abp.ModalManager(abp.appPath + 'BookingService/AssetCategories/AssetCategory/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'BookingService/AssetCategories/AssetCategory/EditModal');

    var dataTable = $('#AssetCategoryTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                                visible: abp.auth.isGranted('EasyAbp.BookingService.AssetCategory.Update'),
                                action: function (data) {
                                    editModal.open({id: data.record.id});
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('EasyAbp.BookingService.AssetCategory.Update'),
                                confirmMessage: function (data) {
                                    return l('AssetCategoryDeletionConfirmationMessage', data.record.id);
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
                title: l('AssetCategoryAssetDefinitionName'),
                data: "assetDefinitionName"
            },
            {
                title: l('AssetCategoryPeriodSchemeId'),
                data: "periodSchemeId"
            },
            {
                title: l('AssetCategoryDefaultPeriodUsable'),
                data: "defaultPeriodUsable"
            },
            {
                title: l('AssetCategoryTimeInAdvance'),
                data: "timeInAdvance"
            },
            {
                title: l('AssetCategoryDisabled'),
                data: "disabled"
            },
            {
                title: l('AssetCategoryCode'),
                data: "code"
            },
            {
                title: l('AssetCategoryLevel'),
                data: "level"
            },
            {
                title: l('AssetCategoryParentId'),
                data: "parentId"
            },
            {
                title: l('AssetCategoryDisplayName'),
                data: "displayName"
            },
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewAssetCategoryButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
