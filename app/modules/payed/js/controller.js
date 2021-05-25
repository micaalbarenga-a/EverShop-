'use strict'
app
    .factory('dataPayed', function ($http, CONFIG) {
        let urlBase = CONFIG.ct_URL;
        return {
            getStatus: ref => $http.get(urlBase + '/pay/GetStatus?id=' + ref)
        }
    })


    .controller("PayedCtrl", function ($scope, $stateParams, dataPayed, $state, $rootScope) {
        var e = $scope.e = {};

        let ref = $stateParams.id;

        function init() {
            dataPayed.getStatus(ref).then(
                ({ data }) => {
                    $rootScope.log = true;
                    e.status = data;
                },
                ({ data }) => {
                    $rootScope.log = true;
                    toaster.warning(data.Message)
                }
            );
        }

        e.try = () => {
            //Reintentar pendiente
            if (e.status.processUrl) {
                window.location.href = data;
            } else {
                //Reintentar rechazado
                $state.go('resume', { order: { OrdId: ref } });
            }

        }

        init()
    });


