<template>
  <div class="div_route">
    <transition name="fade" appear>
      <div
        class="modal-overlay"
        v-if="$store.state.isShowQ6Route"
        @click="$emit('hideModal')"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div class="my_modal" v-if="$store.state.isShowQ6Route">
        <div class="content-top">
          <b>
            {{
              $store.state.language == "En"
                ? "Route information:  "
                : "Thông tin lưu trình:  "
            }}
            &nbsp; &nbsp; route Code:

            <b class="text-route">{{ $store.state.routeCode }}</b>
            &nbsp; &nbsp; route name:
            <b class="text-route">{{ $store.state.routeName }}</b>
          </b>
          &nbsp;
          <!-- <button class="btn btn-primary btn-search" @click="ShowDetailRoute()">
            <Icon icon="search-plus" class="sidenav-icon" />
          </button> -->
          &nbsp; &nbsp;
          <span class="text-search" @click="redirectUrl()"
            >Search more route</span
          >
          <span class="closeButton" @click="$emit('hideModal')"
            ><Icon style="font-size: 23px; color: red" icon="times-circle"
          /></span>
        </div>
        <div class="content" id="content"></div>
      </div>
    </transition>
  </div>
</template>
<script>
import $ from "jquery";
import { mapState } from "vuex";
// import axios from "axios";
export default {
  props: {
    isShowModal: Boolean,
    listDetail: Array,
  },
  computed: mapState(["isShowQ6Route"]),
  watch: {
    isShowQ6Route(newValue) {      
       if (newValue === true) {
         this.ShowDetailRoute();
       }     
    },
  },
  components: {},
  data() {
    return {
      showModal: false,
      isASC: false,
    };
  },
  methods: {
    redirectUrl() {
      window.open("http://10.220.81.121/sfc/Route");
    },
    async ShowDetailRoute() {
      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-NEXT-GROUP",
        route: this.$store.state.routeCode,
        group_name: 0,
      };
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(function (response) {
          return response.json();
        })
        .then(async function (data) {
          if (data != false && data != null) {
            console.log(`loading...`);
          }
        });
      // $("#content").css("width", "1300px");
      await this.getRoute(this.$store.state.routeCode);
    },
    async getRoute(route_code) {
      var mine = this;
      $("#content").html("");
      $("#tb_data").remove();
      await $("body,html").css("cursor", "progress");
      var mainArray = [];
      var ReturnArray = [];
      var specialArray = [];
      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-NEXT-GROUP",
        route: route_code,
        group_name: 0,
      };
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(function (response) {
          return response.json();
        })
        .then(async function (data) {
          $("#content").html("");
          $("#tb_data").remove();
          if (data != false && data != null) {
            var return_data = await data.data;
            if (return_data.length > 0) {
              await mine.addDataTable();
              var groupName = await return_data[0].GROUP_NEXT;
              //$('#content').html(groupName+' - ');
              await mine.addTdData("line_main", groupName);
              await mainArray.push(groupName);
              var RS = await mine.getSpecialGroup(route_code, groupName);
              if (RS != false) {
                await specialArray.push(RS);
              }
              var eor = false;
              var result = false;
              var RG = false;
              // var i = 0;
              await console.log(groupName + " ---luat");
              try {
                while (eor == false) {
                  result = await mine.getGroupNext(route_code, groupName);
                  RG = await mine.getRGroup(route_code, groupName);
                  if (result == false) {
                    eor = await true;
                  } else {
                    groupName = await result[0].GROUP_NEXT;
                    await mainArray.push(groupName);
                  }
                  if (RG != false) {
                    for (var iCount = 0; iCount < RG.length; iCount++) {
                      await ReturnArray.push(RG[iCount]);
                    }
                  }
                  RS = await mine.getSpecialGroup(route_code, groupName);
                  if (RS != false) {
                    await specialArray.push(RS);
                  }
                }
              } catch {
                result = false;
              }
              //Draw
              var vtop = $("#tb_data").offset().top;
              var vleft = $("#tb_data").offset().left;
              $("#myCanvas").attr("height", $("#tb_data").css("height"));
              $("#myCanvas").attr("width", $("#tb_data").css("width"));
              $("#myCanvas").offset({ top: vtop, left: vleft });
              var c = document.getElementById("myCanvas");
              var ctx = c.getContext("2d");
              ctx.font = "40pt Arial";
              ctx.lineWidth = 5;

              ctx.strokeStyle = "#0000ff";
              //------------------------------------------------------------------------------------------------------------------------
              for (let i = 0; i < mainArray.length - 1; i++) {
                await mine.drawLineGroupToGroup(
                  "main",
                  0,
                  ctx,
                  vtop,
                  vleft,
                  mainArray[i],
                  mainArray[i + 1]
                );
              }
              ctx.lineWidth = 2.5;

              ctx.strokeStyle = "#0F0";
              var itemHeight =
                $("#line_main")[0].offsetHeight +
                $("#line_over")[0].offsetHeight;
              //===============================================================================================================================
              for (let i = 0; i < ReturnArray.length; i++) {
                await mine.drawLineGroupToGroup(
                  "re",
                  itemHeight,
                  ctx,
                  vtop,
                  vleft,
                  ReturnArray[i].R,
                  ReturnArray[i].G
                );
                // $("#" + ReturnArray[i].R).attr(
                //   "title",
                //   "-Return to: " + ReturnArray[i].G + " \n"
                // );
              }
              //r_gROUP
              ctx.strokeStyle = "#18c900";
              ctx.strokeStyle = "#F00";
              for (let i = 0; i < ReturnArray.length; i++) {
                await mine.drawLineGroupToGroup(
                  "R",
                  itemHeight,
                  ctx,
                  vtop,
                  vleft,
                  ReturnArray[i].R,
                  ReturnArray[i].F
                );
              }
              //Special line
              ctx.strokeStyle = "#000080";
              for (let i = 0; i < specialArray.length; i++) {
                var sg = specialArray[i];
                for (var j = 0; j < sg.length; j++) {
                  await mine.drawLineGroupToGroup(
                    "S",
                    sg[j].LEVEL,
                    ctx,
                    vtop,
                    vleft,
                    sg[j].G,
                    sg[j].GN
                  );
                  $("#" + sg[j].G).attr(
                    "title",
                    $("#" + sg[j].G).attr("title") +
                      "-Special to: " +
                      sg[j].GN +
                      " \n"
                  );
                }
              }
              //  await  $('#tb_data').css('position', 'absolute');
              await $("#tb_data").css("height", $("#myCanvas").attr("height"));
              await $("#line_over").css("width", "100px");
              await $("#content").css(
                "height",
                parseInt($("#myCanvas").attr("height")) + 50 + "px"
              );
              //End Draw
            } else $("#content").html("No data!");
          } else {
            alert("WebAPI is not response");
            return false;
          }
        });
      await $("body,html").css("cursor", "auto");
    },
    async getGroupNext(route_code, groupName) {
      var mine = this;
      var result = false;
      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-NEXT-GROUP",
        route: route_code,
        group_name: groupName,
      };
      //console.log(BASE1 + JSON.stringify(dynamix));
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(async function (response) {
          return await response.json();
        })
        .then(async function (data) {
          if (data != false && data != null) {
            var return_data = await data.data;
            if (return_data.length > 0) {
              await $.each(return_data, function (key) {
                //    //var gn = return_data[key].GROUP_NEXT;
                //    //$('#content').html($('#content').html()+ gn+' - ');
                mine.addTdData("line_main", return_data[key].GROUP_NEXT);
                //console.log(return_data[key].GROUP_NEXT);
              });
              result = await return_data;
              //await console.log(data);
            }
          } else {
            alert("WebAPI is not response");
          }
        });
      // await console.log(result[0].GROUP_NEXT);
      return await result;
      //await console.log("turn " + result + JSON.stringify(data));
    },
    async drawLineGroupToGroup(
      type,
      topFrom,
      ctx,
      vtop,
      vleft,
      FromRGroupId,
      ToGroupID
    ) {
      if (ToGroupID != "" && FromRGroupId != "") {
        var G = $("#" + ToGroupID);
        var R = $("#" + FromRGroupId);
        var G_X, G_Y, R_X, R_Y;
        if (
          typeof G.offset() === "undefined" ||
          typeof R.offset() === "undefined"
        ) {
          console.log(`....`);
        } else {
          await ctx.beginPath();
          if (type == "S") {
            G_X = parseFloat(G.offset().left - vleft);
            G_Y = parseFloat(G.offset().top - vtop);
            R_X = parseFloat(R.offset().left - vleft);
            R_Y = G_Y;
            var centerG = G_X + parseFloat(G[0].offsetWidth / 2);
            var centerR = R_X + parseFloat(R[0].offsetWidth / 2);
            var level = 0,
              pos = 10;
            if (topFrom == 1) {
              pos = 6;
              level = 45;
              // level=90;
            } else if (topFrom == 2) {
              pos = 8;
              level = 20;
              // level=30;
            } else if (topFrom == 3) {
              pos = 9;
            }
            var c1 = parseFloat((centerR - centerG) / pos);
            if (c1 < 0) c1 = -c1;
            var c2 = centerG - c1;
            c1 = centerR + c1;
            await ctx.moveTo(centerR, R_Y);
            await ctx.bezierCurveTo(c1, level, c2, level, centerG, G_Y);
          } else {
            if (type == "main") {
              G_X = parseInt(G.offset().left - vleft);
              G_Y = parseInt(G.offset().top - vtop + G[0].offsetHeight / 2);
              R_X = parseInt(R.offset().left - vleft + R[0].offsetWidth);
              R_Y = G_Y;
            } else {
              G_X = parseInt(G.offset().left - vleft + G[0].offsetWidth / 2);
              G_Y = parseInt(topFrom); //parseInt(G.offset().top-vtop-G[0].offsetWidth);
              R_X = parseInt(R.offset().left - vleft + R[0].offsetWidth / 2);
              R_Y = parseInt(R.offset().top - vtop);
              if (type == "R") {
                G_X = G_X - 2;
                R_X = R_X - 2;
              }
            }
            await ctx.moveTo(G_X + 1, G_Y);
            await ctx.lineTo(R_X - 1, R_Y);
          }
          await ctx.stroke();
        }
      }
    },
    async getSpecialGroup(route_code, groupName) {
      var result = false;
      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-SPECIAL-GROUP",
        route: route_code,
        group_name: groupName,
      };
      // console.log(BASE1 + JSON.stringify(dynamix));
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          if (data != false && data != null) {
            var return_data = data.data;
            if (return_data.length > 0) {
              result = [];
              var level = 1;
              $.each(return_data, function (key, value) {
                result.push({
                  G: value.GROUP_NAME,
                  GN: value.GROUP_NEXT,
                  LEVEL: level,
                });
                level++;
              });
            }
          } else {
            alert("WebAPI is not response");
          }
        });
      return await result;
    },
    async getRGroup(route_code, groupName) {
      var mine = this;
      var result = false;

      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-R-GROUP",
        route: route_code,
        group_name: groupName,
      };
      // console.log(BASE1 + JSON.stringify(dynamix));
      var list_Return_group = [];
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(function (response) {
          return response.json();
        })
        .then(async function (data) {
          var rGroup = "";
          // returnGroup = "",
          // same = false;
          if (data != false && data != null) {
            var return_data = data.data;
            if (return_data.length > 0) {
              rGroup = await return_data[0].GROUP_NEXT;
              //returnGroup= getReturnRGroup(route_code,rGroup);
              list_Return_group = await mine.getReturnRGroup(
                route_code,
                rGroup
              );
              //if(returnGroup==groupName) same=true;
            }
          } else {
            alert("WebAPI is not response");
          }
          if (rGroup != "") {
            for (let iCount = 0; iCount < list_Return_group.length; iCount++) {
              //same=false;
              if (list_Return_group[iCount] == groupName) {
                await mine.addTdData("line_relation", "", "relation_return");
              } else {
                await mine.addTdData("line_relation", "", "relation");
              }
            }
            // if(same==true)
            // addTdData('line_relation','','relation_return');
            // else{
            // addTdData('line_relation','','relation');
            // }
          } else {
            await mine.addTdData("line_relation", "");
          }
          await mine.addTdData("line_r", rGroup);
          if (rGroup != "") {
            //Draw
            //R: R_ group name
            //G: return back from R_ group
            //F: group Name before R_
            if (list_Return_group.length > 0) {
              result = [];
              for (
                let iCount = 0;
                iCount < list_Return_group.length;
                iCount++
              ) {
                result[iCount] = await {
                  R: rGroup,
                  G: list_Return_group[iCount],
                  F: groupName,
                };
              }
            }
            //result= {R:rGroup,G:returnGroup,F:groupName};
            //End Draw
          }
        });
      return await result;
    },
    async getReturnRGroup(route_code, groupName) {
      var rGroup = "";
      var list_R_group = [];
      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-NEXT-GROUP",
        route: route_code,
        group_name: groupName,
      };
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(function (response) {
          return response.json();
        })
        .then(async function (data) {
          if (data != false && data != null) {
            var return_data = data.data;
            // var iCount = 0;
            if (return_data.length > 0) {
              for (let iCount = 0; iCount < return_data.length; iCount++) {
                rGroup = await return_data[iCount].GROUP_NEXT;
                list_R_group[iCount] = await rGroup;
              }
            }
          } else {
            alert("WebAPI is not response");
          }
        });
      //return rGroup;
      return await list_R_group;
    },
    addDataTable() {
      var table = document.createElement("table");
      table.id = "tb_data";
      var line_over = document.createElement("tr");
      line_over.id = "line_over";
      var line_main = document.createElement("tr");
      line_main.id = "line_main";
      var line_relation = document.createElement("tr");
      line_relation.id = "line_relation";
      var line_r = document.createElement("tr");
      line_r.id = "line_r";
      table.appendChild(line_over);
      table.appendChild(line_main);
      table.appendChild(line_relation);
      table.appendChild(line_r);
      document.getElementById("content").appendChild(table);
      var canvas = document.createElement("canvas");
      canvas.id = "myCanvas";
      canvas.innerHTML = "Your browser does not support the HTML5 canvas tag.";
      document.getElementById("content").appendChild(canvas);
    },
    addTdData(line_name, tdContent, relation) {
      var f_relation = relation == undefined ? false : true;
      var td = document.createElement("td");
      // <img src="@Url.Content("~/Pc2.ico")" height="25px" width="25px"/>
      //~/Content/img/Pc2.ico
      if (tdContent != "")
        td.innerHTML =
          '<div class="" id="' +
          tdContent +
          '" title="" onclick="showGroupInfo(this);"><div class="contentRoute"><label>' +
          tdContent +
          "</label></div></div>";
      else td.innerHTML = "<span>" + tdContent + "</span>";
      if (f_relation == true) {
        td.className = relation;
      } else {
        if (tdContent != undefined && tdContent != "") {
          td.className = "have_data";
        }
      }
      // $('#tdContent').addClass("border");
      document.getElementById(line_name).appendChild(td);
      var td_space = document.createElement("td");
      td_space.innerHTML = "";
      td_space.className = "space";
      document.getElementById(line_name).appendChild(td_space);
    },
  },
};
</script>
<style lang="scss" >
#tb_data{
  color: black;
}
.contentRoute {
  border: 1px solid #0959a2;
  border-radius: 10%;
  font-size: 13px;
  padding: 0 10px;
  font-weight: 555;
  background-color: #d0e9fd;
  margin: auto;
  text-align: center;
  padding-top: 5px;
}
#line_relation {
  // height: 500px !important;
  min-height: 100px !important;
  td {
    height: 250px !important;
    // min-height: 500px !important;
  }
}
#tb_data tr {
  text-align: left;
  /* line-height: 4em; */
}

#tb_data tr td {
  /* padding: 0 10px; */
  text-align: center;
  /* padding-right:1em;; 
	border:1px solid #ccc; */
  white-space: nowrap;
}

#tb_data tr .space {
  min-width: 1em;
}

#tb_data tr .have_data {
  /* background-color:#ccc;	 */
  /* border:1px dotter #8a8a8a; */
  padding: 5px 0;
}

#tb_data tr .have_data span {
  cursor: pointer;
}

#tb_data tr .relation,
#tb_data tr .relation_return {
  background-repeat: no-repeat;
  // background-repeat-y: initial;
  background-position: center;
  height: 100%;
  width: 100%;
  line-height: 100%;
}
#tb_data {
  //height: 400px;
  /* max-height:100px; */
  width: 1%;
  z-index: 100;
}
#line_main {
  position: relative;
  /* background-co
    lor:#ccc; */
  /* background-image: url(bg_line_main.png); */
  background-repeat: no-repeat;
  // background-repeat-x: initial;
  background-position: center;
}
#line_over {
  height: 80px !important;
}
#tb_data {
  z-index: 1000 !important;
  height: 350px !important;
}
#content {
  height: 70vh !important;
  width: 90vw;
  overflow-y: hidden;
}
.space {
  width: 60px !important;
}
</style>
<style lang="scss" scoped>
.text-search {
  text-decoration: underline;
  font-size: 14px;
  color: #fa255d;
  font-weight: 555;
  cursor: pointer;
  user-select: none;
  &:hover {
    color: #ba375a;
  }
}
.text-route {
  color: #418bca;
}
.img-excel {
  height: 45px;
  width: 70px;
  &:hover {
    cursor: pointer;
  }
}
.closeButton {
  margin-top: -15px;
  margin-right: -15px;
  &:hover {
    cursor: pointer;
  }
}
.count-number {
  color: red;
}
.content-top {
  display: flex;
  justify-content: space-between;
}

body {
  font-family: "montserrat", sans-serif;
}

#app {
  position: absolute;

  display: flex;
  justify-content: center;
  align-items: center;
  width: 100vw;
  min-height: 100vh;
  overflow-x: hidden;
}

.button {
  appearance: none;
  outline: none;
  border: none;
  background: none;
  cursor: pointer;

  display: inline-block;
  padding: 15px 25px;
  background-image: linear-gradient(to right, #cc2e5d, #ff5858);
  border-radius: 8px;

  color: #fff;
  font-size: 18px;
  font-weight: 700;

  box-shadow: 3px 3px rgba(0, 0, 0, 0.4);
  transition: 0.4s ease-out;

  &:hover {
    box-shadow: 6px 6px rgba(0, 0, 0, 0.6);
  }
}

.modal-overlay {
  position: fixed;
  z-index: 99990 !important;
  height: 100vh;
  width: 100vw;
  // position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 99;
  background-color: rgba(0, 0, 0, 0.3);
}

.my_modal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 99991 !important;
  z-index: 99;
  height: 85vh;
  width: 100%;
  max-width: 95vw;
  background-color: #fff;
  border-radius: 16px;

  padding: 25px;

  h1 {
    color: #222;
    font-size: 32px;
    font-weight: 900;
    margin-bottom: 15px;
  }

  p {
    color: #666;
    font-size: 18px;
    font-weight: 400;
    margin-bottom: 15px;
  }
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}

.fade-enter,
.fade-leave-to {
  opacity: 0;
}

.slide-enter-active,
.slide-leave-active {
  transition: transform 0.5s;
}

.slide-enter,
.slide-leave-to {
  transform: translateY(-50%) translateX(100vw);
}
</style>
