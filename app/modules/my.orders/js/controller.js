﻿'use strict'
app
    .factory('dataMyOrders', function ($http, CONFIG) {
        let urlBase = CONFIG.ct_URL;
        return {
            get: (name, mail, mobile) => $http.get(urlBase + '/order/MyOrders'),
            pay: (id, user) => $http.get(urlBase + '/pay/GetUrl?id=' + id + '&userAgent=' + user)
        }
    })


    .controller("MyOrdersCtrl", function ($scope, dataMyOrders, toaster, $rootScope, $state) {
        var e = $scope.e = {};

        e.init = () => {
            e.isLoading = true;
            dataMyOrders.get().then(({ data }) => {
                e.isLoading = false;
                e.data = data;
            })
        }

        e.pay = (id) => {

            dataMyOrders.pay(id, $rootScope.userAgent).then(
                ({ data }) => {
                    window.location.href = data;
                },
                ({ data }) => toaster.warning(data.Message)
            );
        }

        e.init()

        if (!$rootScope.log)
            $state.go("login");
    })
    ;

