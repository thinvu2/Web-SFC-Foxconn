import $ from 'jquery'
import { createStore } from 'vuex'
function comparer(otherArray) {
    return function (current) {
        return otherArray.filter(function (other) {
            return other.VALUE == current.VALUE
        }).length == 0;
    }
}
export default createStore({
    state: {
        //apiAddress: "https://sfc.efoxconn.com/api/",

        //apiAddress: "http://10.224.130.70/api/",
        //apiAddress : "http://10.224.81.154/api/",
        //const apiAddress = "http://10.225.35.123/myapi/";

       apiAddress: "https://sfcnic-cns.myfiinet.com/websfcapi",
        //apiAddress: "https://10.220.96.220:8080/websfcapi",
        //apiAddress: 'http://10.220.96.223:8080/webquerysfcapi',
       // apiAddress: "http://localhost:55829/",

        language: 'En',
        isShowModal: false,
        listDetailClick: [],
        listDetailClickAll: [],
        listDetailClickHeader: [],
        isPayLoad: false,
        isLoadingAxios: false,
        isShowQMModel: false,
        isShowModalConfig7: false,//dung cho config 7
        isShowQMMO: false,
        isShowQMLine: false,
        isShowQMGroup: false,
        isShowConfig43: false,//dung cho config43
        isShowModelLock: false,
        isShowLockEmpPass: false,
        isDisableLockElement: false,
        isShowLockUnLock: false,
        isShowSystemLog: false,
        isLockStation: false,
        isSelectOne: false,
        is_lock_edit: '',
        routeInfo: {},
        isShowMixChart: false,
        isShowPieChart: false,
        chartData: [],
        mixPieSeries: [],
        mixPieOption: {
            chart: {
                width: 500,
                type: 'pie',
            },
            labels: [],
            responsive: [{
                breakpoint: 480,
                options: {
                    chart: {
                        width: 200
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }]
        },
        mixChartSeries: [
            {
                name: 'left',
                type: "column",
                data: [],
            },
            {
                name: 'right',
                type: "line",
                data: [],
            },
        ],
        mixChartOption: {
            chart: {
                height: 480,
                type: 'line',
            },
            stroke: {
                width: [0, 4]
            },
            title: {
                text: 'Demo chart'
            },
            dataLabels: {
                enabled: true,
                enabledOnSeries: [1]
            },
            labels: [],
            xaxis: {
                type: 'number'
            },
            yaxis: [{
                title: {
                    text: 'Website Blog',
                },
                max: {
                    value: 100,
                },

            }, {
                opposite: true,
                title: {
                    text: 'Social Media'
                }, max: {
                    value: 400,
                }
            }]
        },
        isShowQ6QueryList: false,
        isShowQ6Route: false,
        isShowQ6Element: false,
        Q6ElementNameEn: 'Enter element',
        Q6ElementNameVi: 'Nh?p thu?c tính',
        routeCode: '',
        routeName: '',
        titleTextChart: 'DemoChart',

        listSelectDualModel: [],
        listSelectDualModelConfig7: [],//dung cho config 7
        listAvaiDualModel: [],
        listAvaiDualModelConfig7: [],//dung cho config7
        listSelectDualMO: [],
        listAvaiDualMO: [],
        listSelectDualLine: [],
        listAvaiDualLine: [],
        listSelectDualGroup: [],
        listSelectDualConfig43: [],//dung cho config 43
        listAvaiDualGroup: [],
        listAvaiDualConfig43: [],//dung cho config43
        listLock: [],
        dateTimeFromQM: '',
        dateTimeToQM: '',
        listAvaiDualModelSearch: [],
        listAvaiDualModelSearchConfig7: [],//dung cho config 7
        listAvaiDualMOSearch: [],
        listAvaiDualLineSearch: [],
        listAvaiDualGroupSearch: [],
        listAvaiDualGroupSearchConfig43: [],//dung cho config43
        lock_emp_pass: '',
        lock_model_name: '',
        lock_group_name: '',
        lock_type: '',
        lock_condition_value: '',
        lock_reason: '',
        lock_solution: '',
        elementQ6: '',
    },
    mutations: {
        showModalLockUnlock(state) {
            state.isShowLockUnLock = true;
        },
        lockStatusModal(state, type) {
            state.isLockStation = type;
        },
        changeLockModalState(state, type) {
            state.is_lock_edit = type;
        },
        updateDateTimeFromQM(state, dateTime) {
            state.dateTimeFromQM = dateTime;
        },
        updateDateTimeToQM(state, dateTime) {
            state.dateTimeToQM = dateTime;
        },
        loadAxios(state) {
            state.isLoadingAxios = true;
        },
        unloadAxios(state) {
            state.isLoadingAxios = false;
        },
        changeLanguage(state, language) {
            state.language = language;
        },
        closeModal(state) {
            state.isShowModal = false;
            state.isShowModalConfig7 = false;//dung cho config 7
            state.isShowQMModel = false;
            state.isShowQMMO = false;
            state.isShowQMLine = false;
            state.isShowQMGroup = false;
            state.isShowConfig43 = false;//dung cho config 43
            state.isShowModelLock = false;
            state.isShowLockEmpPass = false;
            state.isDisableLockElement = false;
            state.isShowLockUnLock = false;
            state.listDetailClick = [];
            state.listDetailClickAll = [];
            state.listDetailClickHeader = [];
            state.chartCategory = [];
            state.chartData = [];
            state.chartMixDataLeft = [];
            state.chartMixDataRight = [];
            state.titleTextChart = '';
            state.isLockStation = false;
            state.isShowSystemLog = false;
            state.isSelectOne = false;
            state.isShowQ6Element = false;
            state.Q6ElementNameEn = 'Enter element';
            state.Q6ElementNameVi = 'Nh?p thu?c tính';
            state.elementQ6 = '';
            state.isShowPieChart = false;
            state.isShowMixChart = false;
            state.isShowQ6Route = false;
            state.routeInfo = {};
            state.isShowQ6QueryList = false;
            state.lock_emp_pass = '';
            state.lock_model_name = '';
            state.lock_group_name = '';
            state.lock_type = 'COUNT';
            state.lock_condition_value = '';
            state.lock_reason = '';
            state.lock_solution = '';
        },
        selectOneElement(state) {
            state.isSelectOne = true;
        },
        ClearListLock(state) {
            state.listLock = [];
        },
        updateListLock(state, newList) {
            state.listLock = newList;
        }
        ,
        deleteItemLock(state, item) {
            var catalogs = state.listLock;
            catalogs = catalogs.filter((p) => {
                if (p.MODEL_NAME == item.MODEL_NAME && p.GROUP_NAME == item.GROUP_NAME && p.TYPE == item.TYPE && p.CONDITION == item.CONDITION) {
                    return false;
                }
                return true;
            });
            state.listLock = catalogs;
        },
        changeLockEmpPass(state, value) {
            state.lock_emp_pass = value;
        },
        changeLockModelName(state, value) {
            state.lock_model_name = value;
        },
        changeLockType(state, value) {
            state.lock_type = value;
        },
        changeLockValue(state, value) {
            state.lock_condition_value = value;
        },
        changeLockGroupName(state, value) {
            state.lock_group_name = value;
        },
        RefreshState(state) {
            state.isLoadingAxios = false;
            state.isShowQMModel = false;
            state.isShowModelConfig7 = false;//dung cho config 7
            state.isShowQMMO = false;
            state.isShowQMLine = false;
            state.isShowQMGroup = false;
            state.isShowConfig43 = false;//dung cho config 43
            state.isShowModelLock = false;
            state.isDisableLockElement = false;
            state.isLockStation = false;
            state.isShowLockUnLock = false;
            state.isSelectOne = false;
            state.isShowQ6Element = false;
            state.isShowQ6Route = false;
            state.chartData = [];
            state.elementQ6 = '';
            state.isShowMixChart = false;
            state.routeInfo = {};
            state.isShowQ6QueryList = false;
            state.isShowSystemLog = false;
            state.lock_emp_pass = '';
            state.lock_model_name = '';
            state.lock_group_name = '';
            state.lock_type = '';
            state.lock_condition_value = '';
            state.lock_reason = '';
            state.lock_solution = '';
            state.listDetailClickHeader = [];
            state.listSelectDualModel = [];
            state.listAvaiDualModel = [];
            state.listSelectDualMO = [];
            state.listAvaiDualMO = [];
            state.listSelectDualLine = [];
            state.listAvaiDualLine = [];
            state.listSelectDualGroup = [];
            state.listSelectDualGroupConfig7 = [];//dung cho config 7
            state.listSelectDualConfig43 = [];//dung cho config43
            state.listAvaiDualGroup = [];
            state.listAvaiDualGroupConfig7 = [],//dung cho config 7
            state.listAvaiDualConfig43 = [];//dung cho config 43
            state.listLock = [],

                state.dateTimeFromQM = '';
            state.dateTimeToQM = '';

            state.listAvaiDualModelSearch = [];
            state.listAvaiDualMOSearch = [];
            state.listAvaiDualLineSearch = [];
            state.listAvaiDualGroupSearch = [];
            state.listAvaiDualGroupConfig7Search = [];//dung cho config 7
            state.listAvaiDualGroupSearchConfig43 = [];//dung cho config 43
        },
        setListAll(state, arr) {
            state.listDetailClickAll = arr;
        },
        disableElementLock(state) {
            state.isDisableLockElement = true;
        },
        showModalEmpPass(state) {
            state.isShowLockEmpPass = true;
            $("body").addClass("modal-open");
        },
        showModalLock(state) {
            state.isShowModelLock = true;
            $("body").addClass("modal-open");
        }
        ,
        showModal(state) {
            state.isShowModal = true;
            $("body").addClass("modal-open");
        },
        showModalModel(state) {
            state.isShowQMModel = true;
            $("body").addClass("modal-open");
        },
        showModalMO(state) {
            state.isShowQMMO = true;
            $("body").addClass("modal-open");
        },
        showModalLine(state) {
            state.isShowQMLine = true;
            $("body").addClass("modal-open");
        },
        showModalGroup(state) {
            state.isShowQMGroup = true;
            $("body").addClass("modal-open");
        },
        showModalModelConfig7(state) {
            state.isShowModalConfig7 = true;
            $("body").addClass("modal-open");
        },
        showConfig43(state) {//dung cho config43
            state.isShowConfig43 = true;
            $("body").addClass("modal-open");
        },
        //--start--QM-select-Model
        UpdateListSelectModel(state, list) {
            if (!state.isSelectOne) {
                if (list.length == 0) {
                    state.listAvaiDualModel = [];
                } else {
                    var array = list;
                    var listSelect = state.listSelectDualModel;
                    array = list.filter(comparer(listSelect));
                    state.listAvaiDualModel = array;
                    state.listAvaiDualModelSearch = array;
                }
            } else {
                if (list.length == 0) {
                    state.listAvaiDualModel = [];
                } else {
                    if (state.listSelectDualModel.length == 0) {
                        array = list;
                        listSelect = state.listSelectDualModel;
                        array = list.filter(comparer(listSelect));
                        state.listAvaiDualModel = array;
                        state.listAvaiDualModelSearch = array;
                    }
                }
            }
        },
        QMListSearchEqualAvaiModel(state) {
            var array = state.listAvaiDualModel;
            var listSelect = state.listSelectDualModel;
            array = array.filter(comparer(listSelect));
            state.listAvaiDualModelSearch = array;

        }, QMFilterModel(state, text) {
            var catalogs = state.listAvaiDualModel;
            catalogs = catalogs.filter(comparer(this.state.listSelectDualModel));
            catalogs = catalogs.filter((p) => { if (p.VALUE.includes(text)) { return true; } return false; });
            state.listAvaiDualModelSearch = catalogs;
        },
        moveAllListAvaiToSelectModel(state) {
            state.listSelectDualModel.push.apply(state.listSelectDualModel, state.listAvaiDualModelSearch);
            state.listAvaiDualModelSearch = [];
        },
        moveAllListSelectToAvaiModel(state) {
            state.listAvaiDualModelSearch.push.apply(state.listAvaiDualModelSearch, state.listSelectDualModel);
            state.listSelectDualModel = [];
        },
        moveItemToSelectModel(state, item) {
            state.listSelectDualModel.push(item);
            var catalogs = state.listAvaiDualModelSearch;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listAvaiDualModelSearch = catalogs;
        },
        moveItemToAvaiModel(state, item) {
            state.listAvaiDualModelSearch.push(item);
            var catalogs = state.listSelectDualModel;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listSelectDualModel = catalogs;
        },
        //----------MO
        //--start--QM-select-MO
        UpdateListSelectMO(state, list) {
            if (list.length == 0) {
                state.listAvaiDualMO = [];
                state.listAvaiDualMOSearch = [];
            } else {
                var array = list;
                var listSelect = state.listSelectDualMO;

                array = list.filter(comparer(listSelect));
                state.listAvaiDualMO = array;
                state.listAvaiDualMOSearch = array;
            }

        }, QMListSearchEqualAvaiMO(state) {
            var array = state.listAvaiDualMO;
            var listSelect = state.listSelectDualMO;
            array = array.filter(comparer(listSelect));
            state.listAvaiDualMOSearch = array;

        }, QMFilterMO(state, text) {
            var catalogs = state.listAvaiDualMO;
            catalogs = catalogs.filter(comparer(this.state.listSelectDualMO));
            catalogs = catalogs.filter((p) => { if (p.VALUE.includes(text)) { return true; } return false; });
            state.listAvaiDualMOSearch = catalogs;
        },
        moveAllListAvaiToSelectMO(state) {
            state.listSelectDualMO.push.apply(state.listSelectDualMO, state.listAvaiDualMOSearch);
            state.listAvaiDualMOSearch = [];
        },
        moveAllListSelectToAvaiMO(state) {
            state.listAvaiDualMOSearch.push.apply(state.listAvaiDualMOSearch, state.listSelectDualMO);
            state.listSelectDualMO = [];
        },
        moveItemToSelectMO(state, item) {
            state.listSelectDualMO.push(item);
            var catalogs = state.listAvaiDualMOSearch;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listAvaiDualMOSearch = catalogs;
        },
        moveItemToAvaiMO(state, item) {
            state.listAvaiDualMOSearch.push(item);
            var catalogs = state.listSelectDualMO;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listSelectDualMO = catalogs;
        },
        //______________Line 
        //--start--QM-select-Line
        UpdateListSelectLine(state, list) {
            if (list.length == 0) {
                state.listAvaiDualLine = [];
            } else {
                var array = list;
                var listSelect = state.listSelectDualLine;

                array = list.filter(comparer(listSelect));
                state.listAvaiDualLine = array;
                state.listAvaiDualLineSearch = array;
            }

        }, QMListSearchEqualAvaiLine(state) {
            var array = state.listAvaiDualLine;
            var listSelect = state.listSelectDualLine;
            array = array.filter(comparer(listSelect));
            state.listAvaiDualLineSearch = array;

        }, QMFilterLine(state, text) {
            var catalogs = state.listAvaiDualLine;
            catalogs = catalogs.filter(comparer(this.state.listSelectDualLine));
            catalogs = catalogs.filter((p) => { if (p.VALUE.includes(text)) { return true; } return false; });
            state.listAvaiDualLineSearch = catalogs;
        },
        moveAllListAvaiToSelectLine(state) {
            state.listSelectDualLine.push.apply(state.listSelectDualLine, state.listAvaiDualLineSearch);
            state.listAvaiDualLineSearch = [];
        },
        moveAllListSelectToAvaiLine(state) {
            state.listAvaiDualLineSearch.push.apply(state.listAvaiDualLineSearch, state.listSelectDualLine);
            state.listSelectDualLine = [];
        },
        moveItemToSelectLine(state, item) {
            state.listSelectDualLine.push(item);
            var catalogs = state.listAvaiDualLineSearch;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listAvaiDualLineSearch = catalogs;
        },
        moveItemToAvaiLine(state, item) {
            state.listAvaiDualLineSearch.push(item);
            var catalogs = state.listSelectDualLine;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listSelectDualLine = catalogs;
        },
        UpdateListSelectGroup(state, list) {

            if (list.length == 0) {
                state.listAvaiDualGroup = [];
            } else {
                var array = list;
                var listSelect = state.listSelectDualGroup;
                array = list.filter(comparer(listSelect));
                state.listAvaiDualGroup = array;
                state.listAvaiDualGroupSearch = array;
            }
        }, QMListSearchEqualAvaiGroup(state) {
            var array = state.listAvaiDualGroup;
            var listSelect = state.listSelectDualGroup;
            array = array.filter(comparer(listSelect));
            state.listAvaiDualGroupSearch = array;
        }, QMFilterGroup(state, text) {
            var catalogs = state.listAvaiDualGroup;
            catalogs = catalogs.filter(comparer(this.state.listSelectDualGroup));
            catalogs = catalogs.filter((p) => { if (p.VALUE.includes(text)) { return true; } return false; });
            state.listAvaiDualGroupSearch = catalogs;
        },
        moveAllListAvaiToSelectGroup(state) {
            state.listSelectDualGroup.push.apply(state.listSelectDualGroup, state.listAvaiDualGroupSearch);
            state.listAvaiDualGroupSearch = [];
        },
        moveAllListSelectToAvaiGroup(state) {
            state.listAvaiDualGroupSearch.push.apply(state.listAvaiDualGroupSearch, state.listSelectDualGroup);
            state.listSelectDualGroup = [];
        },
        moveItemToSelectGroup(state, item) {
            state.listSelectDualGroup.push(item);
            var catalogs = state.listAvaiDualGroupSearch;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listAvaiDualGroupSearch = catalogs;
        },
        moveItemToAvaiGroup(state, item) {
            state.listAvaiDualGroupSearch.push(item);
            var catalogs = state.listSelectDualGroup;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listSelectDualGroup = catalogs;
        },
                //______________Config7
        //--start--Config7
        // UpdateListSelectConfig7Group(state, list) {
        //     if (list.length == 0) {
        //         state.listAvaiDualGroupConfig7 = [];
        //     } else {
        //         var array = list;
        //         var listSelect = state.listSelectDualGroupConfig7;
        //         array = list.filter(comparer(listSelect));
        //         state.listAvaiDualGroupConfig7 = array;
        //         state.listAvaiDualGroupConfig7Search = array;
        //     }
        // },

        UpdateListSelectModelConfig7(state, list){
            if (!state.isSelectOne) {
                if (list.length == 0) {
                    state.listAvaiDualModelConfig7 = [];
                } else {
                    var array = list;
                    var listSelect = state.listSelectDualModelConfig7;
                    array = list.filter(comparer(listSelect));
                    state.listAvaiDualModelConfig7 = array;
                    state.listAvaiDualModelSearchConfig7 = array;
                }
            } else {
                if (list.length == 0) {
                    state.listAvaiDualModelConfig7 = [];
                } else {
                    if (state.listSelectDualModelConfig7.length == 0) {
                        array = list;
                        listSelect = state.listSelectDualModelConfig7;
                        array = list.filter(comparer(listSelect));
                        state.listAvaiDualModelConfig7 = array;
                        state.listAvaiDualModelSearchConfig7 = array;
                    }
                }
            }
        },

        Config7ListSearchEqualAvaiGroup(state) {
            var array = state.listAvaiDualModelConfig7;
            var listSelect = state.listSelectDualModelConfig7;
            array = array.filter(comparer(listSelect));
            state.listAvaiDualModelSearchConfig7 = array;
        },
        Config7FilterGroup(state, text) {
            var catalogs = state.listAvaiDualModelConfig7;
            catalogs = catalogs.filter(comparer(this.state.listSelectDualModelConfig7));
            catalogs = catalogs.filter((p) => { if (p.VALUE.includes(text)) { return true; } return false; });
            state.listAvaiDualModelSearchConfig7 = catalogs;
        },
        moveAllListAvaiToSelectGroupConfig7(state){
            state.listSelectDualModelConfig7.push.apply(state.listSelectDualModelConfig7, state.listAvaiDualModelSearchConfig7);
            state.listAvaiDualModelSearchConfig7 = [];
        },
        moveAllListSelectToAvaiGroupConfig7(state) {
            state.listAvaiDualModelSearchConfig7.push.apply(state.listAvaiDualModelSearchConfig7, state.listSelectDualModelConfig7);
            state.listSelectDualModelConfig7 = [];
        },
        moveItemToSelectGroupConfig7(state, item) {
            state.listSelectDualModelConfig7.push(item);
            var catalogs = state.listAvaiDualModelSearchConfig7;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listAvaiDualModelSearchConfig7 = catalogs;
        },
        moveItemToAvaiGroupConfig7(state, item) {
            state.listAvaiDualModelSearchConfig7.push(item);
            var catalogs = state.listSelectDualModelConfig7;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listSelectDualModelConfig7 = catalogs;
        },
        //dung cho config 43
        UpdateListSelectConfig43(state, list) {
            // if (list.length == 0) {
            //     state.listAvaiDualConfig43 = [];
            // } else {
            //     var array = list;
            //     var listSelect = state.listSelectDualConfig43;
            //     array = list.filter(comparer(listSelect));
            //     state.listAvaiDualConfig43 = array;
            //     state.listAvaiDualGroupSearchConfig43 = array;
            // }

            if (!state.isSelectOne) {
                if (list.length == 0) {
                    state.listAvaiDualConfig43 = [];
                } else {
                    var array = list;
                    var listSelect = state.listAvaiDualConfig43;
                    array = list.filter(comparer(listSelect));
                    state.listAvaiDualConfig43 = array;
                    state.listAvaiDualGroupSearchConfig43 = array;
                }
            } else {
                if (list.length == 0) {
                    state.listAvaiDualConfig43 = [];
                } else {
                    if (state.listAvaiDualConfig43.length == 0) {
                        array = list;
                        listSelect = state.listAvaiDualConfig43;
                        array = list.filter(comparer(listSelect));
                        state.listAvaiDualConfig43 = array;
                        state.listAvaiDualGroupSearchConfig43 = array;
                    }
                }
            }
        },
        Config43ListSearchEqualAvaiGroup(state) {
            var array = state.listAvaiDualConfig43;
            var listSelect = state.listSelectDualConfig43;
            array = array.filter(comparer(listSelect));
            state.listAvaiDualGroupSearchConfig43 = array;
        },
        Config43FilterGroup(state, text) {
            var catalogs = state.listAvaiDualConfig43;
            catalogs = catalogs.filter(comparer(this.state.listSelectDualConfig43));
            catalogs = catalogs.filter((p) => { if (p.VALUE.includes(text)) { return true; } return false; });
            state.listAvaiDualGroupSearchConfig43 = catalogs;
        },
        moveAllListAvaiToSelectConfig43(state) {
            state.listSelectDualConfig43.push.apply(state.listSelectDualConfig43, state.listAvaiDualGroupSearchConfig43);
            state.listAvaiDualGroupSearchConfig43 = [];
        },
        moveAllListSelectToAvaiConfig43(state) {
            state.listAvaiDualGroupSearchConfig43.push.apply(state.listAvaiDualGroupSearchConfig43, state.listSelectDualConfig43);
            state.listSelectDualGroupConfig7 = [];
        },
        moveItemToSelectConfig43(state, item) {
            state.listSelectDualConfig43.push(item);
            var catalogs = state.listAvaiDualGroupSearchConfig43;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listAvaiDualGroupSearchConfig43 = catalogs;
        },
        moveItemToAvaiConfig43(state, item) {
            state.listAvaiDualGroupSearchConfig43.push(item);
            var catalogs = state.listSelectDualConfig43;
            catalogs = catalogs.filter((p) => p.VALUE != item.VALUE);
            state.listSelectDualConfig43 = catalogs;
        },
        //end dung cho config 43
    },
    actions: {},
    modules: {}
})