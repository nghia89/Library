/// <reference path="angular.js" />
// Define the `LibraryApp` module

var app = angular.module('LibraryApp', []);

// Define the `BookGenresCtrlr` controller on the `LibraryApp` module
app.controller('BookGenresCtrlr', function ($scope, $http) {

    $scope.GetAllData = function (id) {
        $http({
            method: "get",
            url: "/TheLoaiSach/Get_AllTheLoaiSach"
        }).then(function (response) {
            $scope.list = response.data;
            for (i = 0; i < response.data.length; i++) {
                if (response.data[i].Id == id) {
                    $scope.IdTheLoai = response.data[i];
                }
            }
        }, function () {
            alert("Error Occur");
        })
    };
});

// Define the `PublishersCtrlr` controller on the `LibraryApp` module
app.controller('PublishersCtrlr', function ($scope, $http) {

    $scope.GetAllData = function (id) {
        $http({
            method: "get",
            url: "/NhaXuatBan/Get_AllNhaXuatBan"
        }).then(function (response) {
            $scope.list = response.data;
            for (i = 0; i < response.data.length; i++) {
                if (response.data[i].Id == id) {
                    $scope.IdNhaXuatBan = response.data[i];
                }
            }
        }, function () {
            alert("Error Occur");
        })
    };
});

// nhập sách
app.controller('ImportBookCtrlr', function ($scope, $http) {

    $scope.list = [];
    $scope.addItem = function () {
        $scope.errortext = "";
        $http({
            method: "get",
            url: "/PhieuNhapSach/_GetBookItemById",
            params: {
                maKS: $scope.maKS,
                soLuong: $scope.soLuong,
                idTrangThai: $scope.idTrangThai,
                GhiChuDon: $scope.GhiChuDon,
            }
        }).then(function (response) {
            if (response.data !== null) {
                $scope.list.push(response.data);
            }
            else {
                alert("Mã sách không phù hợp");
            }
        }, function (e) {
            alert("bạn vui lòng điền vào ô trống");
        })
    }
    $scope.removeItem = function (x) {
        $scope.errortext = "";
        $scope.list.splice(x, 1);
    };

});

//xuất sách
app.controller('ExportBookCtrlr', function ($scope, $http) {

    $scope.list = [];
    $scope.addItema = function () {
        $scope.errortext = "";
        $http({
            method: "get",
            url: "/PhieuXuatSach/_GetBookItemById",
            params: {
                maKiemSoat: $scope.maKS,
                soLuong: $scope.soLuong,
                idTrangThai: $scope.idTrangThai,
                LyDo: $scope.LyDo,
                GhiChu: $scope.GhiChu
            }
        }).then(function (response) {
            if (response.data !== null) {
                $scope.list.push(response.data);
            }
            else {
                alert("Mã sách không phù hợp");
            }
        }, function () {
            alert("bạn vui lòng điền vào ô trống");
        })
    }
    $scope.removeItem = function (x) {
        $scope.errortext = "";
        $scope.list.splice(x, 1);
    };

});

// Get a book by Id - show bookName 
app.controller('MuonSachChooseBookCtrlr', function ($scope, $http) {
    $scope.list = [];

    $scope.addListItems = function (lstSach) {
        if (lstSach !== null) {
            for (i = 0; i < lstSach.length; i++) {
                $scope.maKiemSoat = JSON.parse(lstSach[i]).MaKiemSoat;
                $scope.addItem();
            }
        }
    }

    $scope.addItem = function () {
        $scope.errortext = "";
        $scope.soLuongTemp = 1;
        $scope.found = false;
        $scope.list.forEach(function (item, idx) {
            if (item.MaKiemSoat === $scope.maKiemSoat) {
                $scope.found = true;
                $scope.soLuongTemp = item.SoLuongMuon + 1;
            }
        });

        $http({
            method: "get",
            url: "/PhieuMuon/_GetBookItemById",
            params: {
                MaKiemSoat: $scope.maKiemSoat,
                soLuong: $scope.soLuongTemp,
            }
        }).then(function (response) {
            if (response.data) {
                if (response.data.Status) {
                    if ($scope.found === true) {
                        $scope.list.forEach(function (item, idx) {
                            if (item.MaKiemSoat === $scope.maKiemSoat)
                                item.SoLuongMuon = response.data.SoLuongMuon
                        });
                    } else {
                        $scope.list.push(response.data);
                    }
                }
                else {
                    alert("Sách quá số lượng");
                }
            }
            else {
                alert("Mã sách không phù hợp");
            }
        }, function () {
            alert("bạn vui lòng điền vào ô trống");
        })
    }

    $scope.updateNumber = function (id) {
        $scope.errortext = "";
        $http({
            method: "get",
            url: "/PhieuMuon/_GetBookItemById",
            params: {
                MaKiemSoat: $scope.list[id].MaKiemSoat,
                soLuong: document.getElementById('sach' + id).value,
            }
        }).then(function (response) {
            if (response.data) {
                // cap nhat lai so luong
                if (response.data.Status !== true) {
                    alert("Sách quá số lượng");
                }
                $scope.list[id].SoLuongMuon = response.data.SoLuongMuon;
                document.getElementById('sach' + id).value = response.data.SoLuongMuon;
            }
            else {
                alert("Mã sách không phù hợp");
            }
        }, function () {
            alert("Error Occur");
        })
    };

    $scope.removeItem = function (x) {
        $scope.errortext = "";
        $scope.list.splice(x, 1);
    };
});

// Get a book by Id - TraSachCtrlr  
/*
HEAD

app.controller('TraSachCtrlr', function ($scope, $http) {

app.controller('TraSachCtrlr_Vinh', function ($scope, $http) {

    $scope.list = [];

    $scope.addItem = function (id) {
        //var id = document.getElementById('idSach').innerHTML; //id ma sach (id mongo)
        var sltra = document.getElementById('sl' + id).value;
        var trangthai = document.getElementById('tt' + id).value;
        var idPM = document.getElementById('idPM').value;

        $http({
            method: "get",
            url: "/PhieuTra/GetThongTinPhieuTra",
            params: {
                id: id,
                soLuong: sltra,
                idTrangThai: trangthai,
                idPM: idPM
            }
        }).then(function (response) {
            if (response.data) {
                var found = false;
                $scope.list.forEach(function (item, idx) {
                    if (item.IdSach === id && item.IdTrangThaiSach === response.data.IdTrangThaiSach) {
                        item = response.data;
                        found = true;
                    }
                });

                if (found === false)
                    $scope.list.push(response.data);
            }
            else {
                alert("Dữ liệu không phù hợp");
            }
        }, function () {
            alert("Error Occur");
        })
    };

    $scope.removeItem = function (x) {
        $scope.errortext = "";
        $scope.list.splice(x, 1);
    };

    //$scope.GetAllData = function () {
    //    $http({
    //        method: "get",
    //        url: "/PhieuMuon/GetChiTietPhieuJSon",
    //        params: {
    //            idPM: $('#idPM').val(),
    //            soLuong: 0 
    //        }
    //    }).then(function (response) {
    //        $scope.listGet = response.data;
    //    }, function () {
    //        alert("Error Occur");
    //    })
    //};
});

app.controller('SachMuonCtrlr', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/PhieuMuon/GetChiTietPhieuJSon",
            params: {
                idPM: $('#idPM').val(),
                soLuong: 0
            }
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});
*/

app.controller('KeSach', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/KeSach/GetAll"
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

app.controller('NgonNgu', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/NgonNgu/GetAll"
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

app.controller('TacGiaSelectorCtrlr', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/TacGia/GetAll"
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

app.controller('MuonSachCtrlr', function ($scope, $http, $filter, $location) {
    $scope.BoolThemSach = false; //Được phép thêm sách
    $scope.show_thongbao = false; //Hiện thị div thông báo (chứa thông tin sách or báo lỗi)
    $scope.show_thongbao_CoSach = false; //True: hiện thông tin sách; false: hiện báo lỗi
    $scope.NoiDungThongBao = "Lỗi"; //Nội dung thông báo lỗi
    $scope.disabled_input = true; //disabled input
    $scope.loading_bar = false; //show loading bar 
    $scope.SoluongSachMuon = 1;
    $scope.list_book_queue = []; //list sách chuẩn bị cho mượn
    $scope.list_book_dangmuon = []; //list sách dang muon cho mượn

    //=====================Mượn Sách========================================================
    //load thông tin sách và ẩn loading bar
    $scope.GetBook_loading = function () {
        $scope.loading_bar = true;
        $scope.show_thongbao = false;
        setTimeout(function () {
            $scope.GetBook();
            $scope.loading_bar = false;
        }, 500);
    };

    //Load list sách user đang mượn - $http
    $scope.GetListBook = function () {
        var IdUser = parseLocation(window.location.search)['IdUser'];
        $http({
            method: "post",
            url: "/MuonSach/GetListBook_IdUser",
            params: {
                IdUser: IdUser
            }
        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;

        }, function () {
            alert("Error Occur");
        })
    };

    //Mượn sách - $http
    $scope.MuonSach = function () {
        var DTO = JSON.stringify($scope.list_book_queue);
        //todo
        //update database
        $http({
            method: "post",
            url: "/MuonSach/UpdateListBook",
            contentType: 'application/json; charset=utf-8',
            data: DTO

        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;
            $scope.ResetListBookQueue();
        }, function () {
            alert("Error Occur");
        })
    }

    //Event nhấp enter trên input[type="text"] nhập masach
    $scope.myFunct = function (keyEvent) {
        if (keyEvent.keyCode == 13) {
            $scope.GetBook_loading();
        }
    };

    //lấy sách thêm vào list
    $scope.GetBook = function () {
        $scope.masach = $("#MaSach").val();
        $http({
            method: "post",
            url: "/MuonSach/GetBook",
            params: {
                maSach: $("#MaSach").val()
            }
        }).then(function (response) {
            $scope.list = response.data;
            var bool_kq = false;
            //Kiểm tra
            if ($scope.list.length > 0) {
                //Số lượng sách còn lại lớn hơn không
                if ((parseInt($scope.list[0].SoLuong) - parseInt($scope.Get_SoluongSachDangMuonTheoMKS($scope.masach))) > 0) {
                    bool_kq = true;
                } else {
                    //Số lượng trong sách hết
                    bool_kq = false;
                    $scope.NoiDungThongBao = "Sách trong kho đã hết";
                }
            } else {
                bool_kq = false;
                $scope.NoiDungThongBao = ($scope.masach == "") ? "Chưa nhập mã sách" : "Mã sách không tồn tại";
            }

            //kết quả
            if (bool_kq) {
                //Kiểm tra MaKiemSoat có tồn tại trong list chưa
                let index = $scope.list_book_queue.findIndex(_ => _.MaKiemSoat == $scope.masach);
                if (index >= 0) {
                    //Đã tồn tại
                    $scope.list_book_queue[index].SoLuong = (parseInt($scope.list_book_queue[index].SoLuong) + parseInt($scope.SoluongSachMuon)).toString();
                    $("#List_" + $scope.list_book_queue[index].MaKiemSoat).val($scope.list_book_queue[index].SoLuong);
                } else {
                    //Chưa tồn tại
                    $scope.items = {
                        Id: "",
                        IdUser: parseLocation(window.location.search)['IdUser'],
                        MaKiemSoat: $scope.list[0].MaKiemSoat,
                        TenSach: $scope.list[0].TenSach,
                        SoLuong: "1",
                        SoLuongMax: (parseInt($scope.list[0].SoLuong)).toString(),
                        NgayMuon: Date.now().toString(),
                        NgayTra: $("#ngayPhaiTra").val(),
                        TinhTrang: false
                    };
                    $scope.list_book_queue.push($scope.items);

                }
            } else {
                $scope.show_thongbao = true;
            }

            $scope.masach = "";
        }, function () {
            alert("Error Occur");
        })
    };

    //=====================End Mượn Sách========================================================

    //load focus input số lượng sách có thể mượn trong giới hạn min max
    $scope.loadfocus_input_soluongsachmuon = function (index, value) {
        $scope.list_book_queue = $filter('orderBy')($scope.list_book_queue, '-NgayMuon');
        var max = $scope.list_book_queue[index].SoLuongMax;
        var mks = $scope.list_book_queue[index].MaKiemSoat;
        if (parseInt(value) >= parseInt(max)) {
            $scope.list_book_queue[index].SoLuong = max;
            $("#List_" + mks).val(max);
        }
        if (parseInt(value) <= 0 || value == "") {
            $scope.list_book_queue[index].SoLuong = "1";
            $("#List_" + mks).val("1");
        }
    };

    //reset form thông tin sách
    $scope.ResetBook = function () {
        $scope.show_thongbao = false; //Hiện thị div thông báo (chứa thông tin sách or báo lỗi)
        $scope.show_thongbao_CoSach = false; //True: hiện thông tin sách; false: hiện báo lỗi
        $scope.disabled_input = true; //disabled input
        $scope.loading_bar = false; //show loading bar 
        $scope.BoolThemSach = false;
        $scope.masach = "";
        $scope.TenSach = "";
        $scope.SoluongSachMuon = 1;
        load_datepicker();
    };

    //xoá item khỏi list_book_queue
    $scope.removeItem = function (x) {
        $scope.list_book_queue = $filter('orderBy')($scope.list_book_queue, '-NgayMuon');
        $scope.list_book_queue.splice(x, 1);
        $scope.ResetBook();
    }

    //Clear list book queue
    $scope.ResetListBookQueue = function () {
        $scope.list_book_queue = []
    };

    //Lấy số lượng sách đang mượn
    $scope.Get_SoluongSachDangMuonTheoMKS = function (mks) {
        let index = $scope.list_book_queue.findIndex(_ => _.MaKiemSoat == mks);
        if (index >= 0) {
            return $scope.list_book_queue[index].SoLuong;
        }
        return 0;
    };

    //lấy parameter url
    var parseLocation = function (location) {
        var pairs = location.substring(1).split("&");
        var obj = {};
        var pair;
        var i;

        for (i in pairs) {
            if (pairs[i] === "") continue;

            pair = pairs[i].split("=");
            obj[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1]);
        }

        return obj;
    };

}).directive('myRepeatDirective', function ($timeout) {
    return function (scope, element, attrs) {
        //Load function sau khi run ng-repeat
        $timeout(function () {
            load_datepicker("ngayPhaiTra_" + scope.x['MaKiemSoat']);

            //update số lượng sách trong list chuẩn bị cho mượn
            $("#List_" + scope.x['MaKiemSoat']).change(function () {
                scope.x['SoLuong'] = $("#List_" + scope.x['MaKiemSoat']).val()
            });

            //update ngày trả sách trong list chuẩn bị cho mượn
            $("#ngayPhaiTra_" + scope.x['MaKiemSoat']).change(function () {
                scope.x['NgayTra'] = $("#ngayPhaiTra_" + scope.x['MaKiemSoat']).val()
            });
        });
    };
});

app.controller('TraSachCtrlr', function ($scope, $http, $filter, $location) {
    $scope.BoolThemSach = false; //Được phép thêm sách
    $scope.show_thongbao = false; //Hiện thị div thông báo (chứa thông tin sách or báo lỗi)
    $scope.show_thongbao_CoSach = false; //True: hiện thông tin sách; false: hiện báo lỗi
    $scope.NoiDungThongBao = "Lỗi"; //Nội dung thông báo lỗi
    $scope.disabled_input = true; //disabled input
    $scope.loading_bar = false; //show loading bar 
    $scope.SoluongSachMuon = 1;
    $scope.list_book_queue = []; //list sách chuẩn bị cho mượn
    $scope.list_book_dangmuon = []; //list sách dang muon cho mượn
    $scope.list_book_TinhTrang = [];



    //=====================Trả Sách========================================================
    //load thông tin sách và ẩn loading bar
    $scope.GetBook_loading = function () {
        $scope.loading_bar = true;
        $scope.show_thongbao = false;
        setTimeout(function () {
            $scope.GetBook();
            $scope.loading_bar = false;
        }, 500);
    };

    //Load list sách user đang mượn - $http
    $scope.GetListBook = function () {
        var IdUser = parseLocation(window.location.search)['IdUser'];
        $http({
            method: "post",
            url: "/TraSach/GetListBook_IdUser",
            params: {
                IdUser: IdUser
            }
        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;

        }, function () {
            alert("Error Occur");
        })
    };

    //Mượn sách - $http
    $scope.TraSach = function () {
        var DTO = JSON.stringify($scope.list_book_queue);
        //todo
        //update database
        $http({
            method: "post",
            url: "/TraSach/UpdateListBook",
            contentType: 'application/json; charset=utf-8',
            data: DTO

        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;
            $scope.ResetListBookQueue();
        }, function () {
            alert("Error Occur");
        })
        $scope.ResetBook();
    }

    //Event nhấp enter trên input[type="text"] nhập masach
    $scope.myFunct = function (keyEvent) {
        if (keyEvent.keyCode == 13) {
            $scope.GetBook_loading();
        }
    };

    //lấy sách thêm vào list
    $scope.GetBook = function () {
        $scope.UpdateListChuanBiTra();
        $http({
            method: "post",
            url: "/TraSach/GetBook",
            params: {
                maSach: $("#MaSach").val(),
                IdUser: parseLocation(window.location.search)['IdUser']
            }

        }).then(function (response) {
            $scope.GetBook_Result(response);
        }, function () {
            alert("Error Occur");
        })
    };

    //lấy sách thêm vào list (2) chọn trên dánhacsh
    $scope.GetBook_2 = function (MaSach, NgayMuon, NgayTra) {
        $scope.show_thongbao = false;
        $scope.UpdateListChuanBiTra();
        $http({
            method: "post",
            url: "/TraSach/GetBook",
            params: {
                maSach: MaSach,
                IdUser: parseLocation(window.location.search)['IdUser'],
                NgayMuon: NgayMuon,
                NgayTra: NgayTra
            }

        }).then(function (response) {
            $scope.GetBook_Result(response);
        }, function () {
            alert("Error Occur");
        })
    };

    //$scope.GetBook True
    $scope.GetBook_Result = function (response) {
        $scope.list = response.data;
        var bool_kq = false;
        //Kiểm tra
        if ($scope.list.length > 0) {
            //Số lượng sách còn lại lớn hơn không
            var a = parseInt($scope.GetTotalSoLuongSach_ViewDangTra($scope.list[0])); /*GET số sách chuẩn bị trả của item*/
            if ((parseInt($scope.list[0].SoLuong) - a) > 0) {
                bool_kq = true;
            } else {
                //Số lượng trong danh sách đang mượn đã được chọn hết
                bool_kq = false;
                $scope.NoiDungThongBao = "Sách trong danh sách đã hết";
            }
        } else {
            //Mã sách không tồn tại trong danh sách đang mượn
            //or đối tượng được chọn đã tồn tại trên list chuẩn bị trả
            bool_kq = false;
            $scope.NoiDungThongBao = "Sách này hiện không thể trả";
        }

        //kết quả
        if (bool_kq) {
            //Kiểm tra MaKiemSoat có tồn tại trong list chưa
            let index = $scope.list_book_queue.findIndex(_ => _.MaKiemSoat == $scope.list[0].MaKiemSoat
                                                            && _.NgayMuon == $scope.list[0].NgayMuon
                                                            && _.NgayTra == $scope.list[0].NgayTra
                                                            && _.TinhTrangSach == $scope.modelTT
                                                            );
            if (index >= 0) {
                //Đã tồn tại
                $scope.list_book_queue[index].SoLuong = (parseInt($scope.list_book_queue[index].SoLuong) + 1).toString();
                $("#List_" + $scope.list_book_queue[index].Id).val($scope.list_book_queue[index].SoLuong); /*Cập nhật value cho input textbox SoLuong*/

            } else {

                //Chưa tồn tại
                var edit_NgayMuon = $scope.list[0].NgayMuon.split('/').join('-'); //format lại date
                var edit_Ngaytra = $scope.list[0].NgayTra.split('/').join('-'); //format lại date
                var index_TT = $scope.list_book_TinhTrang.findIndex(_=>_.Id == $scope.modelTT); //lấy thông tin của tình trạng đang chọn
                $scope.items = {
                    Id: $scope.list[0].MaKiemSoat + edit_NgayMuon + edit_Ngaytra + $scope.modelTT,
                    IdUser: parseLocation(window.location.search)['IdUser'],
                    MaKiemSoat: $scope.list[0].MaKiemSoat,
                    TenSach: $scope.list[0].TenSach,
                    SoLuong: "1",
                    SoLuongMax: (parseInt($scope.list[0].SoLuong)).toString(),
                    NgayMuon: $scope.list[0].NgayMuon,
                    NgayTra: $scope.list[0].NgayTra,
                    TinhTrangSach: $scope.modelTT,
                    TinhTrangSachTen: $scope.list_book_TinhTrang[index_TT].TenTT,
                    TinhTrang: false
                };
                $scope.list_book_queue.push($scope.items);

            }
        } else {
            $scope.show_thongbao = true;
        }
        $scope.GetAllTrangThaiSach();
        $scope.masach = "";
    }

    //update list chuẩn bị trả tới controller
    $scope.UpdateListChuanBiTra = function () {
        var DTO = JSON.stringify($scope.list_book_queue);
        $http({
            method: "post",
            url: "/TraSach/UpdateList_ChuanBiTra",
            contentType: 'application/json; charset=utf-8',
            data: DTO
        }).then(function (response) {
            return true;
        }, function () {
            alert("Error Occur");
            return false;
        })
    }

    //=====================End Trả Sách========================================================

    $scope.GetAllTrangThaiSach = function () {
        $http({
            method: "post",
            url: "/TraSach/GetAllTrangThaiSach",
        }).then(function (response) {
            $scope.list_book_TinhTrang = response.data;
            if ($scope.list_book_TinhTrang.length > 0) {
                $scope.modelTT = $scope.list_book_TinhTrang[0].Id;
            }

        }, function () {
            alert("Error Occur");
            return false;
        })
    }

    //load focus input số lượng sách có thể mượn trong giới hạn min max
    $scope.loadfocus_input_soluongsachmuon = function (index, value) {
        $scope.list_book_queue = $filter('orderBy')($scope.list_book_queue, '-NgayMuon');
        var max = $scope.list_book_queue[index].SoLuongMax;
        var Id = $scope.list_book_queue[index].Id;
        var SlTotal_maSach = parseInt($scope.GetTotalSoLuongSach_ViewDangTra($scope.list_book_queue[index]));

        //value lớn hơn max
        if (parseInt(value) > parseInt(max)) {
            $scope.list_book_queue[index].SoLuong = max;
            $("#List_" + Id).val(max);
        }

        //SlTotal_maSach lớn hơn max (Tổng số lượng sách trả lớn hơn số lượng sách mượn)
        if (SlTotal_maSach > parseInt(max)) {
            $scope.list_book_queue[index].SoLuong = parseInt(max) - (SlTotal_maSach - parseInt(value));
            $("#List_" + Id).val(parseInt(max) - (SlTotal_maSach - parseInt(value)));
        }

        //value nhỏ hơn 0
        if (parseInt(value) <= 0 || value == "") {
            $scope.list_book_queue[index].SoLuong = "1";
            $("#List_" + Id).val("1");
        }
    };

    //reset form thông tin sách
    $scope.ResetBook = function () {
        $scope.show_thongbao = false; //Hiện thị div thông báo (chứa thông tin sách or báo lỗi)
        $scope.show_thongbao_CoSach = false; //True: hiện thông tin sách; false: hiện báo lỗi
        $scope.disabled_input = true; //disabled input
        $scope.loading_bar = false; //show loading bar 
        $scope.BoolThemSach = false;
        $scope.masach = "";
        $scope.TenSach = "";
        $scope.SoluongSachMuon = 1;
        load_datepicker();
        $scope.GetAllTrangThaiSach();
    };

    //xoá item khỏi list_book_queue
    $scope.removeItem = function (x) {
        $scope.list_book_queue = $filter('orderBy')($scope.list_book_queue, '-NgayMuon');
        $scope.list_book_queue.splice(x, 1);
        $scope.ResetBook();
    }

    //Clear list book queue
    $scope.ResetListBookQueue = function () {
        $scope.list_book_queue = []
    };

    //Lấy số lượng sách đang mượn
    $scope.Get_SoluongSachDangMuonTheoMKS = function (mks, ngaymuon, ngaytra) {
        let index = $scope.list_book_queue.findIndex(_ => _.MaKiemSoat == mks && _.NgayMuon == ngaymuon && _.NgayTra == ngaytra);
        if (index >= 0) {
            return $scope.list_book_queue[index].SoLuong;
        }
        return 0;
    };

    //lấy parameter url
    var parseLocation = function (location) {
        var pairs = location.substring(1).split("&");
        var obj = {};
        var pair;
        var i;

        for (i in pairs) {
            if (pairs[i] === "") continue;

            pair = pairs[i].split("=");
            obj[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1]);
        }

        return obj;
    };

    //lấy tổng số lượng sách trên list_book_queue by maSach
    $scope.GetTotalSoLuongSach_ViewDangTra = function (item) {
        var list = $scope.list_book_queue.filter(function (element) {
            return element.MaKiemSoat == item.MaKiemSoat
                && element.NgayMuon == item.NgayMuon
                && element.NgayTra == item.NgayTra;
        });
        var slSach_bool = true;
        var Tong_slSach_DangTra = 0;
        //forEach
        angular.forEach(list, function (value, key) {
            Tong_slSach_DangTra = parseInt(Tong_slSach_DangTra) + parseInt(value.SoLuong);
        });
        return Tong_slSach_DangTra;
    }

    //init
    $scope.GetAllTrangThaiSach();

}).directive('myRepeatDirective', function ($timeout) {
    return function (scope, element, attrs) {
        //Load function sau khi run ng-repeat
        $timeout(function () {
            load_datepicker("ngayPhaiTra_" + scope.x['MaKiemSoat']);

            //update số lượng sách trong list chuẩn bị cho mượn
            $("#List_" + scope.x['Id']).change(function () {
                scope.x['SoLuong'] = $("#List_" + scope.x['Id']).val()
            });

            //update ngày trả sách trong list chuẩn bị cho mượn
            $("#ngayPhaiTra_" + scope.x['MaKiemSoat']).change(function () {
                scope.x['NgayTra'] = $("#ngayPhaiTra_" + scope.x['MaKiemSoat']).val()
            });

            //update ngày trả sách trong list chuẩn bị cho mượn
            $("#Select_" + scope.x['Id']).change(function () {
                scope.x['TinhTrangSach'] = $("#Select_" + scope.x['Id']).val()
            });
        });
    };
});

//======================================Thống Kê=======================================
app.controller('ThongKeCtrlr', function ($scope, $http) {
    $scope.KeyMonth = '';
    $scope.KeyYear = '';
    function GetMonth() {
        $http({
            method: "post",
            url: "/ThongKe/MuonSachJson",
            params: {
                month: $scope.KeyMonth,
                year: $scope.KeyYear
            }
        }).then(function (response) {
            if (response.data !== null) {
                $scope.ListMonthMuonSach = response.data;
                DemSoNguoiMuonSach();
            }
        })
    }
    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    $scope.SelectedMonth = month.toString();
    $scope.SelectedYear = year.toString();
    $scope.GetDataMonth = function () {
        key1 = $scope.SelectedMonth;
        $scope.KeyMonth = key1;

        key2 = $scope.SelectedYear;
        $scope.KeyYear = key2;
        GetMonth();
    }
    GetMonth();
    /*===============================*/
    $scope.Day = '';
    function GetDay() {
        $http({
            method: "post",
            url: "/ThongKe/MuonSachJson",
            params: {
                day: $scope.Day,
            }
        }).then(function (response) {
            if (response != null) {
                $scope.ListDayMuonSach = response.data;
                DemSoNguoiMuonSach();
            }
        })
    }
    $scope.SelectedDay = new Date;
    $scope.GetDataDay = function () {
        key = $scope.SelectedDay;
        $scope.Day = key;
        GetDay();
    }

    $scope.GetDayFirst = function () {
        $scope.Day = $("#day").val();
        GetDay();
    }
    GetDay();
    //-----
    function DemSoNguoiMuonSach() {
        $http({
            method: "post",
            url: "/ThongKe/MuonSachJson_ssdm",
        }).then(function (response) {
            if (response != null) {
                $scope.SoSachDuocMuon = response.data[0];
                $scope.SoNguoiMuonSach = response.data[1];
            }
        })
    }
})
//======================================END Thống Kê===================================