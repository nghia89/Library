var app = angular.module('MyApp', ['chart.js']);

app.run(function ($rootScope) {
    
    // Load Color

    Chart.defaults.global.elements.line.fill = false;
    Chart.defaults.global.colors = [
            {
                backgroundColor: 'rgba(150,122,218,0.8)',
                borderColor: 'rgba(150,122,218,1)',
                pointBackgroundColor: 'rgba(169,151,232,1)',
                pointBorderColor: '#fff',
                pointHoverBackgroundColor: 'rgba(169,151,232,0.5)',
                pointHoverBorderColor: 'rgba(169,151,232,0.8)'
            },
            {
                backgroundColor: 'rgba(53,187,155,0.8)',
                borderColor: 'rgba(53,187,155,1)',
                pointBackgroundColor: 'rgba(83,210,178,1)',
                pointBorderColor: '#fff',
                pointHoverBackgroundColor: 'rgba(83,210,178,0.5)',
                pointHoverBorderColor: 'rgba(83,210,178,0.8)'
            }, {
                backgroundColor: 'rgba(67,155,218,0.8)',
                borderColor: 'rgba(67,155,218,1)',
                pointBackgroundColor: 'rgba(95,182,235,1)',
                pointBorderColor: '#fff',
                pointHoverBackgroundColor: 'rgba(95,182,235,0.5)',
                pointHoverBorderColor: 'rgba(95,182,235,0.8)'
            }, {
                backgroundColor: 'rgba(231,127,76,0.8)',
                borderColor: 'rgba(231,127,76,1)',
                pointBackgroundColor: 'rgba(240,140,99,1)',
                pointBorderColor: '#fff',
                pointHoverBackgroundColor: 'rgba(240,140,99,0.5)',
                pointHoverBorderColor: 'rgba(240,140,99,0.8)'
            }, {
                backgroundColor: 'rgba(245,186,69,0.8)',
                borderColor: 'rgba(245,186,69,1)',
                pointBackgroundColor: 'rgba(250,202,102,1)',
                pointBorderColor: '#fff',
                pointHoverBackgroundColor: 'rgba(250,202,102,0.5)',
                pointHoverBorderColor: 'rgba(250,202,102,0.8)'
            }];
});

app.controller('Statistic', function ($scope, $timeout, $http, $location) {


    $scope.labels = ['Tháng 01', 'Tháng 02', 'Tháng 03', 'Tháng 04', 'Tháng 05', ' Tháng 06', 'Tháng 07', 'Tháng 08', 'Tháng 09', ' Tháng 10', 'Tháng 11', 'Tháng 12'];
    $scope.series = [ 'Số người mượn', 'Số sách mượn', 'Số sách trả', 'Số sách trả trễ hạn'];
    $scope.chartdataYear = [];
    $scope.Year = '';
    $scope.loading = true;
    $scope.SumSl = 0;
    $scope.sumSixMonth = 0;

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

                //var lsoPhieuMuonTrongNam = [];
                var lsoNguoiMuonSachTrongNam = [];
                var lsoSachDuocMuonTrongNam = [];
                var lsoSachDuocTraTrongNam = [];            
                var lsoNguoiTraTreTrongNam = [];

                //response.data.lsoPhieuMuonTrongNam.forEach(function (i, index) {
                //    lsoPhieuMuonTrongNam.push(i);
                //});
                response.data.lsoNguoiMuonSachTrongNam.forEach(function (i, index) {
                    lsoNguoiMuonSachTrongNam.push(i);
                });
                response.data.lsoSachDuocMuonTrongNam.forEach(function (i, index) {
                    lsoSachDuocMuonTrongNam.push(i);
                });
                response.data.lsoSachDuocTraTrongNam.forEach(function (i, index) {
                    lsoSachDuocTraTrongNam.push(i);
                });
                response.data.lsoNguoiTraTreTrongNam.forEach(function (i, index) {
                    lsoNguoiTraTreTrongNam.push(i);
                });
                
                //chartData.push(lsoPhieuMuonTrongNam);
                chartData.push(lsoNguoiMuonSachTrongNam);
                chartData.push(lsoSachDuocMuonTrongNam);
                chartData.push(lsoSachDuocTraTrongNam);           
                chartData.push(lsoNguoiTraTreTrongNam);

                //chartData.push(benefits);
                //chartData.push(benefits);

                $scope.chartdataYear = chartData;

                var highest = lsoNguoiMuonSachTrongNam[0];

                //for (var i = 0; i < lsoPhieuMuonTrongNam.length; i++) {
                //    if (highest < lsoPhieuMuonTrongNam[i]) highest = lsoPhieuMuonTrongNam[i];
                //}
                for (var i = 0; i < lsoNguoiMuonSachTrongNam.length; i++) {
                    if (highest < lsoNguoiMuonSachTrongNam[i]) highest = lsoNguoiMuonSachTrongNam[i];
                }
                for (var i = 0; i < lsoSachDuocMuonTrongNam.length; i++) {
                    if (highest < lsoSachDuocMuonTrongNam[i]) highest = lsoSachDuocMuonTrongNam[i];
                }
                for (var i = 0; i < lsoSachDuocTraTrongNam.length; i++) {
                    if (highest < lsoSachDuocTraTrongNam[i]) highest = lsoSachDuocTraTrongNam[i];
                }
                for (var i = 0; i < lsoNguoiTraTreTrongNam.length; i++) {
                    if (highest < lsoNguoiTraTreTrongNam[i]) highest = lsoNguoiTraTreTrongNam[i];
                }
                highest = highest + (5 - highest % 5);

                $scope.options = {
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                var label = data.datasets[tooltipItem.datasetIndex].label || '';

                                if (label) {
                                    label += ': ';
                                }
                                label += Math.round(tooltipItem.yLabel * 100) / 100;
                                return label;
                            }
                        }
                    },
                    legend: { display: true },
                    scales: {
                        yAxes: [
                            {
                                ticks: {
                                    beginAtZero: true,
                                    suggestedMax: highest,
                                    stepSize: 5,
                                    callback: function (value) { if (value % 1 === 0) { return value; } }

                                },

                            }
                        ]
                    }
                };
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

    function GetInformation() {
        $http({
            method: "get",
            url:"/Statistic/DataInformation",
        }).then(function (response) {
            $scope.SumSl = response.data.SumSoLuong;
            $scope.sumSixMonth = response.data.sixMonthsBack;
        })
    }

    getStatistic();
    GetInformation();
    $scope.loading = false;
});

app.controller('MonthCtroller', function ($scope, $http, $location) {

    $scope.labels = [];
    $scope.series = ['Số người mượn', 'Số sách mượn', 'Số sách trả', 'Số sách trả trễ hạn'];
    
    $scope.chartdataMonth = [];
    $scope.Keymonth = '';
    $scope.KeyYear = '';
    $scope.loading = true;
    
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
                var SoNgayTrongThang = [];

                //var lsoPMTrongNgay = [];
                var lsoNguoiMuonTrongNgay = [];
                var lsoSachDuocMuonTrongNgay = [];   
                var lsoSachDuocTraTrongNgay = [];
                var lsoNguoiTraTreTrongNgay = [];

                
                response.data.lsoNguoiMuonTrongNgay.forEach(function (i, index) {
                    lsoNguoiMuonTrongNgay.push(i);
                });
                response.data.lsoSachDuocMuonTrongNgay.forEach(function (i, index) {
                    lsoSachDuocMuonTrongNgay.push(i);
                });
               
                response.data.lsoSachDuocTraTrongNgay.forEach(function (i, index) {
                    lsoSachDuocTraTrongNgay.push(i);
                });

                response.data.lsoNguoiTraTreTrongNgay.forEach(function (i, index) {
                    lsoNguoiTraTreTrongNgay.push(i);
                });

              

                var highest = lsoNguoiMuonTrongNgay[0];

                //for (var i = 0; i < lsoPMTrongNgay.length; i++) {
                //    if (highest < lsoPMTrongNgay[i]) highest = lsoPMTrongNgay[i];
                //}
              
                for (var i = 0; i < lsoNguoiMuonTrongNgay.length; i++) {
                    if (highest < lsoNguoiMuonTrongNgay[i]) highest = lsoNguoiMuonTrongNgay[i];
                }
                for (var i = 0; i < lsoSachDuocMuonTrongNgay.length; i++) {
                    if (highest < lsoSachDuocMuonTrongNgay[i]) highest = lsoSachDuocMuonTrongNgay[i];
                }
                for (var i = 0; i < lsoSachDuocTraTrongNgay.length; i++) {
                    if (highest < lsoSachDuocTraTrongNgay[i]) highest = lsoSachDuocTraTrongNgay[i];
                }
                for (var i = 0; i < lsoNguoiTraTreTrongNgay.length; i++) {
                    if (highest < lsoNguoiTraTreTrongNgay[i]) highest = lsoNguoiTraTreTrongNgay[i];
                }
                highest = highest + (5 - highest % 5);

                //chartData1.push(lsoPMTrongNgay);
                chartData1.push(lsoNguoiMuonTrongNgay);
                chartData1.push(lsoSachDuocMuonTrongNgay);
                chartData1.push(lsoSachDuocTraTrongNgay);
                chartData1.push(lsoNguoiTraTreTrongNgay);


                for (var x = 0; x <= response.data.SoNgayTrongThang; x++) {
                    SoNgayTrongThang.push(x);
                }  
                $scope.chartdataMonth = chartData1;
                $scope.labels = SoNgayTrongThang;
                $scope.options = {
                    legend: { display: true },
                    scales: {
                        yAxes: [
                            {
                                ticks: {
                                    beginAtZero: true,
                                    suggestedMax: highest,
                                    stepSize: 5,
                                    callback: function (value) { if (value % 1 === 0) { return value; } }
                                }
                            }
                        ]
                    }
                };
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

    function GetInformation() {
        $http({
            method: "get",
            url: "/Statistic/DataInformation",
        }).then(function (response) {
            $scope.SumSl = response.data.SumSoLuong;
            $scope.sumSixMonth = response.data.sixMonthsBack;
        })
    }

    getStatisticMonth();
    GetInformation() 
    $scope.loading = false;

});

app.controller('startDayAndlastDay', function ($scope, $http, $location) {
    $scope.labels = [];
    $scope.series = ['Số người mượn', 'Số sách mượn', 'Số sách trả', 'Số sách trả trễ hạn'];

    $scope.chartdataMonth = [];
    //$scope.startDayAndlastDay = $scope.Datetime;
    $scope.loading = true;
    $scope.Datetime = '';
    $scope.series = ['Số người mượn', 'Số sách mượn', 'Số sách trả', 'Số sách trả trễ hạn'];
    $scope.labels = ['0','1','2','3','4','5'];
    $scope.chartdataDay = ['8','9','20','10','12','13'];

    function Day() {
        var config={
            param: {
                dateTime: $scope.Datetime
            }
        }
        $http({
            method: 'get',
            url: '/Statistic/StartDayAndlastDay?&dateTime='+config.param.dateTime,
        }).then(function (res) {
            var first = '01/01/2018';
            var last='0'
        })
    }
    var date = new Date();
    var curr_date = date.getDate();
    var curr_month = date.getMonth();
    curr_month++;
    var cur_time = date.getHours();
    var curr_year = date.getFullYear();
    $scope.Datetime = curr_month + "/" + curr_date + "/" + curr_year;

    $scope.GetDay = function () {
        $scope.Datetime = $scope.Datetime;
        Day();
    }

    Day();
});

app.controller('TTSach', function ($scope, $http, $location) {
    $scope.labels = [];
    $scope.series = ['Số người mượn', 'Số sách mượn', 'Số sách trả', 'Số sách trả trễ hạn'];

    $scope.chartdataMonth = [];
    //$scope.startDayAndlastDay = $scope.Datetime;
    $scope.loading = true;
    $scope.Datetime = '';
    $scope.series = ['Số người mượn', 'Số sách mượn', 'Số sách trả', 'Số sách trả trễ hạn'];
    $scope.labels = ['0', '1', '2', '3', '4', '5'];
    $scope.chartdataDay = ['8', '9', '20', '10', '12', '13'];

    function TKSachTT() {
        var config = {
            param: {
              
            }
        }
        $http({
            method: 'get',
            url: '/Statistic/TTSach',
        }).then(function (res) {

        })
    }
   
    TKSachTT();
});
