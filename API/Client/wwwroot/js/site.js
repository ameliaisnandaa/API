// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// Write your JavaScript code.

/*let animals = [
    { name: "dory", species: "fish", class: { name: "invertebrata" } },
    { name: "simba", species: "cat", class: { name: "mamalia" } },
    { name: "tori", species: "cat", class: { name: "mamalia" } },
    { name: "nemo", species: "fish", class: { name: "invertebrata" } },
    { name: "budi", species: "cat", class: { name: "mamalia" } }
]
let OnlyCat = [];

for (i = 0; i < animals.length; i++) {
    if (animals[i].species == "cat") {
        OnlyCat.push(animals[i]);
    }
}
console.log("OnlyCat\n", OnlyCat);
for (i = 0; i < animals.length; i++) {
    if (animals[i].species == "fish") {
        animals[i].class.name = "non-mamalia"
    }
}
console.log("Animals\n", animals);*/
/*
$.ajax({
    url: "https://swapi.dev/api/people/",
    success: function (result) {
        console.log(result.results);
        text = ""; 
        $.each(result.results, function (key,val) {
            text += `<li>${val.hair_color}</li>`;
        })
        console.log(text);
        $('#listSW').html(text);
    }
})*/


/*$.ajax({
    url: "https://swapi.dev/api/people/",
    success: function (result) {
        console.log(result.results);
        text = "";
        $.each(result.results, function (key, val) {
            text += `<tr>
                        <td>${val.name}</td>
                        <td>${val.skin_color}</td>
                        <td>${val.hair_color}</td>
                        <td>${val.birth_year}</td>
                        <td>${val.gender}</td>
                    </tr>`;
        })
        console.log(text);
        $('.tableSW').html(text);
    }
})*/

/*$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon/",
    success: function (result) {
        console.log(result.results);
        text = "";
        $.each(result.results, function (key, val) {
            text += `<tr>
                        <td>${key+1}</td>
                        <td>${val.name}</td>
                        <td><button class="btn btn-primary" onclick="detailSW('${val.url}')" data-target="#modalSW" data-toggle="modal">Action</button></td>
                    </tr>`;
        })
        console.log(text);
        $('.tableSW').html(text);
    }
})*/

/*function detailSW(url) {
    $.ajax({
        url: url,
        success: function (result) {
            console.log(result);
            text = "";
            text = `
                    <table class="table table-hover">
                        <thead class="alert-dark">
                            <tr>
                                <td>Name</td>
                                <td>Weight</td>
                            </tr>
                        </thead>
                        <tbody class="tableSW">
                             <tr>
                                <td>${result.name}</td>
                                <td>${result.weight}</td>
                            </tr>
                        </tbody>
                    </table>
                    `
            $('.modal-body').html(text);
        }
    })
}*/

function detailSW(url) {
    ability = "";
    badge = "";
    stat = "";
    move = "";
    $.ajax({
        url: url,
        success: function (result) {
            result.abilities.forEach(ab => {
                ability += `${ab.ability.name}\n`
            })
            result.moves.forEach(mo => {
                move += `<li>${mo.move.name}</li><>`
            })
            result.stats.forEach(st => {

                stat += `<tr>
                            <td>${st.stat.name}</td>
                            <td class="col-8">
                                <div class="progress">
                                  <div class="progress-bar progress-bar-striped" role="progressbar" style="width: ${st.base_stat}%" aria-valuenow="10" aria-valuemin="0" aria-valuemax="100">${st.base_stat}</div>
                                </div>
                            </td>  
                        </tr>`
            })
            result.types.forEach(t => {
                type = `${t.type.name}`
                if (type == "poison") {
                    badge += `<span class="badge badge-pill badge-dark text-light">${type}</span>`
                }
                else if (type == "grass") {
                    badge += `<span class="badge badge-pill badge-success">${type}</span>`
                }
                else if (type == "normal") {
                    badge += `<span class="badge badge-pill badge-secondary">${type}</span>`
                }
                else if (type == "fire") {
                    badge += `<span class="badge badge-pill badge-danger">${type}</span>`
                }
                else if (type == "water") {
                    badge += `<span class="badge badge-pill badge-primary">${type}</span>`
                }
                else if (type == "bug") {
                    badge += `<span class="badge badge-pill badge-info">${type}</span>`
                }
                else if (type == "flying") {
                    badge += `<span class="badge badge-pill badge-muted">${type}</span>`
                }
            })
            console.log(result);
            text = "";
            text = `
                    <div class="row justify-content-center">
                        <h1>${result.name.toUpperCase()}</h1>
                    </div>
                    <div class="row text-center">
                        <img src="${result.sprites.other.dream_world.front_default}" alt="" class="rounded-circle img-fluid mx-auto d-block shadow-lg">
                    </div>
                    <div class="row justify-content-center my-3">
                        ${badge}
                    </div>
                    <ul class="nav nav-tabs" id="myTab" role="tablist">
                      <li class="nav-item">
                        <a class="nav-link active" id="detail-tab" data-toggle="tab" href="#detail" role="tab" aria-controls="home" aria-selected="true">Detail</a>
                      </li>
                      <li class="nav-item">
                        <a class="nav-link" id="stat-tab" data-toggle="tab" href="#stat" role="tab" aria-controls="profile" aria-selected="false">Stat</a>
                      </li>
                      <li class="nav-item">
                        <a class="nav-link" id="move-tab" data-toggle="tab" href="#move" role="tab" aria-controls="profile" aria-selected="false">Moves</a>
                      </li>
                    </ul>
                    <div class="tab-content" id="myTabContent">
                        <div class="tab-pane fade show active" id="detail" role="tabpanel" aria-labelledby="home-tab">
                            <table class="table table-hover table-striped">
                                <tr>
                                    <td>Name</td>
                                    <td class="col-8">: ${result.name}</td>
                                </tr>
                                <tr>
                                    <td>Ability</td>
                                    <td class="col-8">: ${ability}</td>
                                </tr>
                                <tr>
                                    <td>Weight</td>
                                    <td class="col-8">: ${result.weight}</td>
                                </tr>
                                <tr>
                                    <td>Height</td>
                                    <td class="col-8">: ${result.height}</td>
                                </tr>
                            </table>
                        </div>
                        <div class="tab-pane fade" id="stat" role="tabpanel" aria-labelledby="profile-tab">
                            <table class="table table-hover ">
                                ${stat}
                            </table>
                        </div>
                        <div class="tab-pane fade" id="move" role="tabpanel" aria-labelledby="move-tab">
                            <table class="table table-hover ">
                                <ol>
                                    ${move}
                                <ol>
                            </table>
                        </div>
                    </div>
                   `
            $('.modal-body').html(text);
        }
    })
}
$(document).ready(function () {
    var no = 1;
    $('#datatableSW').DataTable({
        "ajax": {
            "url": "https://pokeapi.co/api/v2/pokemon/",
            "dataType": "json",
            "dataSrc":"results"
        },
        "columns": [
            {
                "data": null,
                render: function (data, type, row) {
                    return `<td>${no++}</td>`
                }
            },
            {
                "data" : "name"
            },
            {
                "data": null,
                render: function (data, type, row) {
                    return `<button class="btn btn-secondary" onclick="detailSW('${row['url']}')" data-target="#modalSW" data-toggle="modal">Action</button>`;
                }
            }
        ]
    });
});

//TAMBAHAN (Lebih menarik, progress, chart, modal > nav>tabs (nav tabs codepan), abilities sama mov pake looping)