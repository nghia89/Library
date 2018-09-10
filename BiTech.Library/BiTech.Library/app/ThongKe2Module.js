
var app = angular.module('LibraryApp', []);

// Define the `BookGenresCtrlr` controller on the `LibraryApp` module
app.controller('Thongke2Ctrlr', function ($scope, $http) {

    $scope.statMode = 0;

    $scope.GetSubDomainList = function (id) {
        $http({
            method: "get",
            url: "/ThongKe2/GetSubDomainList",
            params: {
                wpid: id
            }
        }).then(function (response) {
            $scope.SubDomainPk = response.data.data;

        }, function () {
            console.log("Thongke2Ctrlr - GetSubDomainList - load fail.");
        })
    };

    $scope.GetStatForDomain = function (subdomain) {
        $http({
            method: "get",
            url: "/ThongKe2/GetStatForDomain",
            params: {
                site: subdomain
            }
        }).then(function (response) {
            console.log(response.data.data);

        }, function () {
            console.log("Thongke2Ctrlr - GetStatForDomain - load fail.");
        })
    };

});