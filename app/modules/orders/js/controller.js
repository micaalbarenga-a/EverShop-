'use strict'
app
    .factory('dataOrders', function ($http, CONFIG) {
        let urlBase = CONFIG.ct_URL;
        return {
            get: () => $http.get(urlBase + '/order/AllOrders')
        }
    })


    .controller("OrdersCtrl", function ($scope, dataOrders, toaster) {
        var e = $scope.e = {};

        function init() {
            dataOrders.get().then(
                ({ data }) => {
                    e.data = data;
                }),
                ({ data }) => toaster.warning(data.Message)
        }

        init();

    })



    ;

