/**
sweetalert js provider
*/
'use strict';

angular.module('oitozero.ngSweetAlert', [])
.factory('SweetAlert', ['$rootScope', function ($rootScope) {

    var swal = window.swal;

    //public methods
    var self = {

        swal: function (arg1, arg2, arg3) {
            $rootScope.$evalAsync(function () {
                if (typeof (arg2) === 'function') {
                    swal(arg1, function (isConfirm) {
                        $rootScope.$evalAsync(function () {
                            arg2(isConfirm);
                        });
                    }, arg3);
                } else {
                    swal(arg1, arg2, arg3);
                }
            });
        },
        success: function (title, message) {
            $rootScope.$evalAsync(function () {
                swal(title, message, 'success');
            });
        },
        error: function (title, message) {
            $rootScope.$evalAsync(function () {
                swal(title, message, 'error');
            });
        },
        warning: function (title, message) {
            $rootScope.$evalAsync(function () {
                swal(title, message, 'warning');
            });
        },
        info: function (title, message) {
            $rootScope.$evalAsync(function () {
                swal(title, message, 'info');
            });
        }
    };

    return self;
}]);

/**
app.js
*/

angular.module('LibraryApp', ['ngAnimate', 'toaster', 'oitozero.ngSweetAlert']);

//.config(['$routeProvider', '$locationProvider', '$compileProvider', function ($routeProvider, $locationProvider, $compileProvider) {
//    /**
//    setup - whitelist, appPath, html5Mode
//    @toc 1.
//    */
//    $locationProvider.html5Mode(false);		//can't use this with github pages / if don't have access to the server
//    // var staticPath ='/';
//    var staticPath;
//    // staticPath ='/angular-services/ngSweetAlert/';		//local
//    // staticPath ='/';		//nodejs (local)
//    staticPath = '/ngSweetAlert/';		//gh-pages
//    var appPathRoute = '/';
//    var pagesPath = staticPath + 'pages/';
//    $routeProvider.when(appPathRoute + 'home', { templateUrl: pagesPath + 'home/home.html' });
//    $routeProvider.otherwise({ redirectTo: appPathRoute + 'home' });
//}]);

var app = angular.module('LibraryApp');

app.controller('BackupRestoreCtrl', ['$scope', 'SweetAlert', 'toaster', '$http', function ($scope, SweetAlert, toaster, $http) {

    $scope.listfiles = [];

    $scope.GetFileList = function () {
        $http({
            method: "GET",
            url: "/QuanLyThuVien/GetBackupFiles"
        }).then(function (response) {
            $scope.listfiles = response.data;
        }, function () {
            console.log("BackupRestoreCtrlr - GetFileList - load fail.");
        });
    };

    $scope.create = function () {
        toaster.pop({ type: 'wait', title: "Đang tạo sao lưu", body: "Xin vui lòng chờ", tapToDismiss: false });
        $.ajax({
            type: 'GET',
            url: '/QuanLyThuVien/CreateBackupFile',
            success: function (resultData) {
                //console.log(resultData);
                toaster.clear();

                if (resultData.Status == 0) {
                    toaster.success({ title: "Sao lưu thành công", body: resultData.Data, timeout: 0 });
                    $scope.GetFileList();
                }
                else {
                    toaster.error({ title: "Sao lưu thất bại", body: resultData.Data });
                }
            }
        });
    };

    $scope.upload = function (x) {
        var Name = $scope.listfiles[x].Name;
        console.log("restore " + Name);
    };

    $scope.download = function (x) {
        var Name = $scope.listfiles[x].Name;
        //console.log("download " + Name);
        window.open("/QuanLyThuVien/DownloadBackupFile?name=" + Name);
    };

    $scope.restore = function (x) {
        var Name = $scope.listfiles[x].Name;
        //console.log("restore " + Name);
        toaster.pop({ type: 'wait', title: "Đang khổi phục dữ liệu", body: "Xin vui lòng chờ", tapToDismiss: false });
        $.ajax({
            type: 'GET',
            url: '/QuanLyThuVien/RestoreBackupFile',
            data: { "name": Name },
            success: function (resultData) {
                //console.log(resultData);
                toaster.clear();
                if (resultData.Status == 0) {
                    toaster.success({ title: "Phục hồi thành công", body: resultData.Data, timeout: 0 });
                    $scope.GetFileList();
                }
                else {
                    toaster.error({ title: "Phục hồi thất bại", body: resultData.Data });
                }
            }
        });
    };

    $scope.remove = function (x) {
        var Name = $scope.listfiles[x].Name;

        SweetAlert.swal({
            title: "Bạn có chắc là muốn xóa " + Name + "?",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Có",
            cancelButtonText: "Không",
            closeOnConfirm: true,
            closeOnCancel: true
        },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: 'POST',
                    url: '/QuanLyThuVien/RemoveBackupFile',
                    data: { "name": Name },
                    success: function (resultData) {
                        //console.log(resultData)
                        if (resultData.Status == 0) {
                            toaster.success({ title: "Xóa thành công", body: resultData.data });
                            $scope.GetFileList();
                        }
                        else {
                            toaster.error({ title: "Xóa thất bại", body: resultData.data });
                        }
                    }
                });
            }
        });
    };
}]);