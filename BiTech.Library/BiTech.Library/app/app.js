
var app = angular.module('MyApp', ['chart.js']);

app.controller('Statistic', function ($scope, $timeout, $http, $location) {

    $scope.labels = ['Tháng 01', 'Tháng 02', 'Tháng 03', 'Tháng 04', 'Tháng 05', ' Tháng 06', 'Tháng 07', 'Tháng 08', 'Tháng 09', ' Tháng 10', 'Tháng 11', 'Tháng 12'];
    $scope.series = ['phiếu Mượn Trong Năm', 'Số Người Mượn Trong Năm', 'Số Người không Trả sách', 'Số Sách Được Mượn', 'Số Người Trả Trể'];
    $scope.options = { legend: { display: true } };
    $scope.chartdataYear = [];
    $scope.Year = '';
    Chart.defaults.global.colors = [
      {
          backgroundColor: 'rgba(255, 255, 255, 0)',
          pointHoverBackgroundColor: 'rgba(0, 0, 0, 0.97)',
          borderColor: 'rgba(0, 8, 245, 1',
          pointBorderColor: '#212529',
          //pointHoverBorderColor: 'rgba(151,187,205,1)'
      }, {
          backgroundColor: 'rgba(255, 255, 255, 0)',
          pointHoverBackgroundColor: 'rgba(0, 0, 0, 0.97)',
          borderColor: 'rgba(171, 0, 245, 1',
          pointBorderColor: '#212529',
          //pointHoverBorderColor: 'rgba((0, 8, 245, 1)'
      }, {
          backgroundColor: 'rgba(255, 255, 255, 0)',
          pointHoverBackgroundColor: 'rgba(0, 0, 0, 0.97)',
          borderColor: 'rgba(0, 199, 0, 0.97',
          pointBorderColor: '#212529',
          //pointHoverBorderColor: 'rgba(151,187,205,1)'
      }, {
          backgroundColor: 'rgba(255, 255, 255, 0)',
          pointHoverBackgroundColor: 'rgba(0, 0, 0, 0.97)',
          borderColor: 'rgba(245, 0, 41, 1',
          pointBorderColor: '#212529',
          //pointHoverBorderColor: 'rgba(151,187,205,1)'
      }, {
          backgroundColor: 'rgba(255, 255, 255, 0)',
          pointHoverBackgroundColor: 'rgba(0, 0, 0, 0.97)',
          borderColor: 'rgba(0, 0, 0, 0.97',
          pointBorderColor: '#212529',
          //pointHoverBorderColor: 'rgba(151,187,205,1)'
      }];
    $scope.loading = true;
  
        function getStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    month: '',
                    year: $scope.Year
                }
            }

            $http({
                method: "get",
                url: "/Statistic/BieuDoPhieuMuon?&month=" + config.param.month + "&year=" + config.param.year,
            }).then(function (response) {
                if (response.data) {

                    var chartData = [];

                    var lsoPhieuMuonTrongNam = [];
                    var lsoNguoiMuonSachTrongNam = [];
                    var lsoNguoiKhongTraTrongNam = [];
                    var lsoSachDuocMuonTrongNam = [];
                    var lsoNguoiTraTreTrongNam = [];

                    response.data.lsoPhieuMuonTrongNam.forEach(function (i, index) {
                        lsoPhieuMuonTrongNam.push(i);
                    });
                    response.data.lsoNguoiMuonSachTrongNam.forEach(function (i, index) {
                        lsoNguoiMuonSachTrongNam.push(i);
                    });
                    response.data.lsoNguoiKhongTraTrongNam.forEach(function (i, index) {
                        lsoNguoiKhongTraTrongNam.push(i);
                    });
                    response.data.lsoSachDuocMuonTrongNam.forEach(function (i, index) {
                        lsoSachDuocMuonTrongNam.push(i);
                    });
                    response.data.lsoNguoiTraTreTrongNam.forEach(function (i, index) {
                        lsoNguoiTraTreTrongNam.push(i);
                    });


                    chartData.push(lsoPhieuMuonTrongNam);
                    chartData.push(lsoNguoiMuonSachTrongNam);
                    chartData.push(lsoNguoiKhongTraTrongNam);
                    chartData.push(lsoSachDuocMuonTrongNam);
                    chartData.push(lsoNguoiTraTreTrongNam);

                    //chartData.push(benefits);
                    //chartData.push(benefits);

                    $scope.chartdataYear = chartData;
                    //$scope.labels = labels;
                }
                else {
                    alert("không thể tải dữ liệu");
                }

            })
            var date = new Date();
            var year = date.getFullYear();
            $scope.selectYear = year.toString();

            $scope.year = function () {
                KeyYear = $scope.selectYear;
                $scope.Year = KeyYear;
                getStatistic();           
            }
           
        }
       
        getStatistic();
        $scope.loading = false;
});



app.controller('MonthCtroller', function ($scope, $http, $location) {


    $scope.labels = ['Ngày 01', 'Ngày 02', 'Ngày 03', 'Ngày 04', 'Ngày 05', ' Ngày 06', 'Ngày 07', 'Ngày 08', 'Ngày 09', ' Ngày 10', 'Ngày 11', 'Ngày 12',
    'Ngày 03', 'Ngày 14', 'Ngày 14', 'Ngày 16', 'Ngày 17', ' Ngày 18', 'Ngày 19', 'Ngày 20', 'Ngày 21', ' Ngày 22', 'Ngày 23', 'Ngày 24',
    'Ngày 25', 'Ngày 26', 'Ngày 27', 'Ngày 28', 'Ngày 29', ' Ngày 30', 'Ngày 31'];
    $scope.series = ['phiếu Mượn Trong Ngày', 'Số Người Mượn Trong Ngày', 'Số Người không Trả sách', 'Số Sách Được Mượn', 'Số Người Trả Trể'];
    //$scope.colors = [{

    //    fillColor: 'rgba(230, 100, 150, 0.8)',
    //    strokeColor: 'rgba(47, 132, 71, 0.8)',
    //    highlightFill: 'rgba(47, 132, 71, 0.8)',
    //    highlightStroke: 'rgba(47, 132, 71, 0.8)'
    //}];

    $scope.chartdataMonth = [];
    $scope.options = { legend: { display: true } };
    $scope.loading = true;
    $scope.Keymonth = '';
    $scope.KeyYear = '';


    function getStatisticMonth() {
        var config = {
            param: {
                //mm/dd/yyyy
                month: $scope.Keymonth,
                year: $scope.KeyYear

            }
        }
        $http({
            method: "get",
            url: "/Statistic/BieuDoPhieuMuon?&month=" + config.param.month + "&year=" + config.param.year,
        }).then(function (response) {
            if (response.data) {
                var chartData1 = [];

                var lsoPMTrongNgay = [];
                var lsoNguoiMuonTrongNgay = [];
                var lsoSachDuocMuonTrongNgay = [];
                var lsoNguoiKhongTraTrongNgay = [];
                var lsoNguoiTraTreTrongNgay = [];

                response.data.lsoPMTrongNgay.forEach(function (i, index) {
                    lsoPMTrongNgay.push(i);
                });
                response.data.lsoNguoiMuonTrongNgay.forEach(function (i, index) {
                    lsoNguoiMuonTrongNgay.push(i);
                });
                response.data.lsoNguoiKhongTraTrongNgay.forEach(function (i, index) {
                    lsoNguoiKhongTraTrongNgay.push(i);
                });
                response.data.lsoSachDuocMuonTrongNgay.forEach(function (i, index) {
                    lsoSachDuocMuonTrongNgay.push(i);
                });
                response.data.lsoNguoiTraTreTrongNgay.forEach(function (i, index) {
                    lsoNguoiTraTreTrongNgay.push(i);
                });


                chartData1.push(lsoPMTrongNgay);
                chartData1.push(lsoNguoiMuonTrongNgay);
                chartData1.push(lsoSachDuocMuonTrongNgay);
                chartData1.push(lsoNguoiKhongTraTrongNgay);
                chartData1.push(lsoNguoiTraTreTrongNgay);

                //chartData.push(benefits);
                //chartData.push(benefits);

                $scope.chartdataMonth = chartData1;
                //$scope.labels = labels;
            }
            else {
                alert("không thể tải dữ liệu");
            }

        })
    }

    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    $scope.selected1 = month.toString();
    $scope.selected2 = year.toString();

    $scope.GetData = function () {  
        langKey1 = $scope.selected1;
        $scope.Keymonth = langKey1;

        langKey2 = $scope.selected2;
        $scope.KeyYear = langKey2;
        getStatisticMonth();
    }
    getStatisticMonth();
    $scope.loading = false;

});

