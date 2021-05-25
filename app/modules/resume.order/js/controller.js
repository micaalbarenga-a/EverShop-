'use strict'
app
    .factory('dataResume', function ($http, CONFIG) {
        let urlBase = CONFIG.ct_URL;
        return {
            pay: (id, user) => $http.get(urlBase + '/pay/GetUrl?id=' + id + '&userAgent='+user)
        }
    })


    .controller("ResumeCtrl", function ($scope, $stateParams, $state, dataResume, $rootScope) {
        var e = $scope.e = {};

        if ($stateParams.order) e.order = $stateParams.order;
        else $state.go('index');

        e.pay = () => {

            //Si la ordern aún se puede pagar, procedemos al pago
            dataResume.pay(e.order.OrdId, $rootScope.userAgent).then(({ data }) => {
                if (data) {

                    window.location.href = data;
                }
            })
        }
    });


