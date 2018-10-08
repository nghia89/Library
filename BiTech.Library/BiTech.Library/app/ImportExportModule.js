angular.module('LibraryApp', ['ngAnimate', 'toaster']);

var app = angular.module('LibraryApp');

app.controller('ImportBookCtrlr', ['$scope', 'toaster', '$http', function ($scope, toaster, $http) {
    $scope.list = [];
    $scope.addItem = function () {
        $scope.errortext = "";
        if (!$scope.GhiChuDon) {
            $scope.GhiChuDon = "";
        }
        $http({
            method: "get",
            url: "/PhieuNhapSach/_GetBookItemById",
            params: {
                maKS: $('#idSach').val(),
                soLuong: $scope.soLuong,
                idTrangThai: $scope.idTrangThai,
                GhiChuDon: $scope.GhiChuDon,
            }
        }).then(function (response) {
            if (response.data.valueOf() != "" || response.data != "") {
                $scope.masach = $scope.maKS;
                //Kiểm trả có tồn tại trong list chưa để cộng dồn
                if ($scope.list.length != 0) {
                    let index = $scope.list.findIndex(_ => _.MaKiemSoat == $scope.masach && _.IdTinhTrang == $scope.idTrangThai);
                    if (index >= 0) {
                        //Đã tồn tại
                        $scope.list[index].soLuong = (parseInt($scope.list[index].soLuong) + parseInt($scope.soLuong)).toString();
                        $("#List_" + $scope.list[index].MaKiemSoat + "_" + $scope.list[index].IdTinhTrang).val($scope.list[index].soLuong);
                        $scope.list[index].GhiChuDon = $scope.GhiChuDon;
                        $("#GhiChu_" + $scope.list[index].MaKiemSoat + "_" + $scope.list[index].IdTinhTrang).val($scope.GhiChuDon);
                        console.log($scope.GhiChuDon);
                    }
                    else
                        $scope.list.push(response.data);
                }
                else
                    $scope.list.push(response.data);
                $('#idSach').focus();
                $scope.maKS = null;
                $scope.soLuong = null;
                $scope.GhiChuDon = null;
                $("#TenSach").val("");
            }
            else {
                $scope.errortext = "Vui lòng ";
                if (!$scope.maKS || $scope.maKS.valueOf() == "") {
                    $scope.errortext += "nhập mã sách ";
                    $('#idSach').focus();
                }

                if ($scope.idTrangThai == null || $scope.idTrangThai.valueOf() == "") {
                    if ($scope.errortext.length != "Vui lòng ".length)
                        $scope.errortext += "- ";
                    $scope.errortext += "chọn trạng thái ";
                }

                if (!$scope.soLuong) {
                    if ($scope.errortext.length != "Vui lòng ".length)
                        $scope.errortext += "- ";
                    $scope.errortext += "nhập số lượng ";
                    if (!$scope.maKS || $scope.maKS.valueOf() == "")
                        $('#idSach').focus();
                    else
                        $('#SoLuong').focus();
                }

                if ($scope.errortext == "") {
                    toaster.error({ title: "Thông tin sai", body: "Cần nhập đúng mã sách" });
                    $('#idSach').focus();
                }
                else {
                    if ($scope.errortext.length != "Vui lòng ".length)
                        toaster.error({ title: "Thiếu thông tin", body: $scope.errortext });
                    else {
                        toaster.error({ title: "Thông tin sai", body: "Mã sách không đúng" });
                        $('#idSach').focus();
                    }
                }
            }
        }, function (e) {
            $scope.errortext = "Vui lòng ";
            if (!$scope.maKS || $scope.maKS.valueOf() == "") {
                $scope.errortext += "nhập mã sách ";
                $('#idSach').focus();
            }

            if ($scope.idTrangThai == null || $scope.idTrangThai == "") {
                if ($scope.errortext.length != "Vui lòng ".length)
                    $scope.errortext += "- ";
                $scope.errortext += "chọn trạng thái ";
            }

            if (!$scope.soLuong) {
                if ($scope.errortext.length != "Vui lòng ".length)
                    $scope.errortext += "- ";
                $scope.errortext += "nhập số lượng\r\n";
                if (!$scope.maKS || $scope.maKS.valueOf() == "")
                    $('#idSach').focus();
                else
                    $('#SoLuong').focus();
            }

            if ($scope.errortext.length > 0) {
                toaster.error({ title: "Thiếu thông tin", body: $scope.errortext });
            }
            else
                toaster.error({ title: "Thêm thất bại", body: e.status + " - " + e.statusText });

            console.log(e);
            if ($scope.errortext == "")
                toaster.error({ title: "Cần nhập đúng mã sách", body: response.Data });
        })
        //$('#idSach').focus();
    }

    //Xóa theo MaKiemSoat va IdTrangThai
    $scope.removeItem = function (x1, x2) {
        $scope.errortext = "";
        let index = $scope.list.findIndex(_ => _.MaKiemSoat == x1 && _.IdTinhTrang == x2);
        $scope.list.splice(index, 1);
    };
    //Clear list book queue
    $scope.ResetListBookQueue = function () {
        $scope.list = []
    };
}]);

//xuất sách
app.controller('ExportBookCtrlr', ['$scope', 'toaster', '$http', function ($scope, toaster, $http) {

    $scope.list = [];
    $scope.listTrangThai = [];
    $scope.maKS_Load = "";
    $scope.list_book_queue = [];

    $scope.addItema = function (x) {

        $scope.masach = $scope.maKS; //ma ca biet        

        if (x == null) {
            let indexID = $scope.masach.indexOf(".");
            if (indexID < 0) {
                $scope.AddToQueue();
                return;
            }
            $scope.maKS_Load = "";
            $scope.list_book_queue = [];
        }
        else {
            $scope.masach = x.MaCaBiet;
            $scope.maKS_Load = x.MaCaBiet;
        }

        let index = $scope.list.findIndex(_ => _.MaCaBiet == $scope.masach);
        if (index < 0 || index == null) { //k tim thay
            $scope.errortext = "";
            $http({
                method: "get",
                url: "/PhieuXuatSach/AddBookToList",
                params: {
                    maCaBiet: $scope.masach,
                }
            }).then(function (response) {
                if (response.data == null || response.data == "") {
                    $scope.errortext += "Số lượng sách xuất cần <= số lượng có trong kho.\n";
                    alert($scope.errortext);
                }
                else {
                    // $scope.masach = $scope.maKS;
                    if (response.data.valueOf() != "") {
                        $scope.list.push(response.data);
                        $('#idSach').focus();
                        $scope.maKS = null;
                        if ($scope.maKS_Load != "") {
                            let ind = $scope.list_book_queue.findIndex(_ => _.MaCaBiet == $scope.masach);
                            $scope.removeItem_Queue(ind);
                        }
                    }
                    else {
                        $scope.errortext = "Vui lòng ";
                        if (!$scope.maKS) {
                            $scope.errortext += "nhập mã sách ";
                            $('#idSach').focus();
                        }

                        if ($scope.errortext == "")
                            toaster.error({ title: "Thông tin sai", body: "Cần nhập đúng mã sách" });
                        else
                            if ($scope.errortext.length != "Vui lòng ".length)
                                toaster.error({ title: "Thiếu thông tin", body: $scope.errortext });
                            else {
                                toaster.error({ title: "Thông tin sai", body: "Mã sách không đúng" });
                                $('#idSach').focus();
                            }
                    }

                }
            }, function (e) {
                $scope.errortext = "Vui lòng nhập đúng mã sách ";
                $("#idSach").val("");
                if ($scope.errortext.length > 0)
                    toaster.error({ title: "Thiếu thông tin", body: $scope.errortext });
                else
                    toaster.error({ title: "Thêm thất bại", body: e.status + " - " + e.statusText });

                console.log(e);
                if ($scope.errortext == "")
                    toaster.error({ title: "Thông tin sai", body: "Cần nhập đúng mã sách" });
            })
            //$('#idSach').focus();
        }
        else {
            toaster.error({ title: "Thông tin sai", body: "Đã tồn tại trong danh sách thanh lý" });
            $("#idSach").val("");
        }
    }

    $scope.AddToQueue = function () {
        $http({
            method: "get",
            url: "/PhieuXuatSach/AddBookToListQueue",
            params: {
                maSach: $('#idSach').val(),
            }
        }).then(function (response) {
            if (response.data == null || response.data == "") {
                toaster.error({ title: "Thông tin sai", body: "Mã sách không đúng" });
            }
            else {
                $scope.list_book_queue = response.data;
                if ($scope.list != null) {
                    for (i = 0; i < $scope.list.length; i++) {
                        var index = $scope.list_book_queue.findIndex(_ => _.MaCaBiet == $scope.list[i].MaCaBiet)
                        if (index >= 0) {
                            $scope.removeItem_Queue(index);
                        }
                    }
                }
            }
        }, function () {
            console.log("Fail - Get book  to queue");
            $scope.errortext = "Vui lòng nhập đúng mã sách ";
            $("#idSach").val("");
            toaster.error({ title: "Thiếu thông tin", body: $scope.errortext });
        })
    }

    $scope.removeItem = function (x1, x2) {
        $scope.errortext = "";
        let index = $scope.list.findIndex(_ => _.MaKiemSoat == x1 && _.IdTinhTrang == x2);
        $scope.list.splice(index, 1);
    };

    $scope.removeItem_Queue = function (x) {
        $scope.list_book_queue.splice(x, 1);
    }

    //Clear list book queue
    $scope.ResetListBookQueue = function () {
        $scope.list = [];
        $scope.list_book_queue = [];
    };
    //Load danh sach trang thai cua sach da co
    $scope.updateListTrangThai = function () {
        $http({
            method: "get",
            url: "/PhieuXuatSach/GetStatusByIdBook",
            params: { idBook: $scope.maKS }
        }).then(function (response) {
            $scope.listTrangThai = response.data;
            console.log(response.data.length);
        })
    };
}]);