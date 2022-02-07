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


$.ajax({
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
})

