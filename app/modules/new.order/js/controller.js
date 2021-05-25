'use strict'
app
    .factory('dataNewOrder', function ($http, CONFIG) {
        let urlBase = CONFIG.ct_URL;
        return {
            setOrder: data => $http.post(urlBase + '/order/Orders', data),
            products: {
                get: () => $http.get(urlBase + '/product/Products')
            }
        }

    })


    .controller("NewOrderCtrl", function ($scope, dataNewOrder, $state, toaster, $rootScope) {
        var e = $scope.e = {};


        function init() {
            //Obtención del listado de productos
            e.isLoading = true;
            dataNewOrder.products.get().then(
                ({ data }) => {
                    e.isLoading = false;
                    e.products = data;
                },
                ({ data }) => {
                    toaster.warning(data.Message);
                    e.isLoading = false;
                }
            )
        }

        e.buy = (p) => {
            //seteamos la orden

            dataNewOrder.setOrder({ Product: p }).then(
                ({ data }) => {
                    if (data) {
                        toaster.success("Orden de compra ingresada con éxito");
                        $state.go('resume', { order: data });
                    }
                },
                ({ data }) => {
                    toaster.warning(data.Message);
                }
            );
        }

        init();

        if (!$rootScope.log)
            $state.go("login");

    })

    //.component('customerData', {
    //    templateUrl: 'modules/new.order/view/customer.html',
    //    controller: function (toaster) {
    //        this.cus = {};

    //        this.already = () => {
    //            if (!this.cus.name) {
    //                toaster.warning("Faltan ingresar el nombre");
    //                return;
    //            }
    //            else if (!this.cus.mail) {
    //                toaster.warning("Faltan ingresar mail");
    //                return;
    //            }
    //            else if (!this.cus.mobile) {
    //                toaster.warning("Faltan ingresar número de teléfono");
    //                return;
    //            }
    //            this.disabled = true;
    //            this.callback(!!(this.cus.name && this.cus.mail && this.cus.mobile), this.cus);
    //        }

    //        this.unload = () => {
    //            this.disabled = false;
    //            this.callback(false);
    //        }
    //    },
    //    bindings: {
    //        callback: '<'
    //    }
    //});
