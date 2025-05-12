class Despesa {
    constructor(ano, mes, dia, tipo, descricao, valor) {
        this.ano = ano
        this.mes = mes
        this.dia = dia
        this.tipo = tipo
        this.descricao = descricao
        this.valor = valor
    }
    validarDados() {
        for (let i in this) {
            if (this[i] == undefined || this[i] == '' || this[i] == null) {
                return false
            }
        }
        return true
    }
}
class Bd {
    constructor() {
        let id = localStorage.getItem('id')
        id = Number(id)
        if (isNaN(id) || id < 0) {
            id = 0
            localStorage.setItem('id', id)
        }
    }
    getProximoId() {
        let id = Number(localStorage.getItem('id'))
        if (isNaN(id) || id < 0) id = 0
        return id + 1
    }
    gravar(d) {
        let id = this.getProximoId()
        localStorage.setItem(id, JSON.stringify(d)) // Armazena a despesa com o ID gerado
        localStorage.setItem('id', id) // Atualiza o ID no localStorage
    }
    recuperarTodosRegistros() {
        let despesas = []
        let id = Number(localStorage.getItem('id'))
        if (isNaN(id) || id < 0) id = 0

        for (let i = 1; i <= id; i++) {
            let despesaStr = localStorage.getItem(i)
            if (despesaStr !== null) {
                try {
                    let despesaObj = JSON.parse(despesaStr)
                    despesaObj.id = i  // adiciona o ID ao objeto
                    despesas.push(despesaObj)
                } catch (e) {
                    console.error(`Erro ao parsear a despesa com ID ${i}:`, e)
                }
            }
        }
        return despesas
    }
    pesquisar(despesa) {
        let despesasFiltradas = Array()
        despesasFiltradas = this.recuperarTodosRegistros()
        console.log(despesa)

        if (despesa.ano != '') {
            console.log('filto de ano')
            despesasFiltradas = despesasFiltradas.filter(d => d.ano == despesa.ano)
        }
        if (despesa.mes != '') {
            console.log('filto de mes')
            despesasFiltradas = despesasFiltradas.filter(d => d.mes == despesa.mes)
        }
        if (despesa.dia != '') {
            console.log('filto de dia')
            despesasFiltradas = despesasFiltradas.filter(d => d.dia == despesa.dia)
        }
        if (despesa.tipo != '') {
            console.log('filto de tipo')
            despesasFiltradas = despesasFiltradas.filter(d => d.tipo == despesa.tipo)
        }
        if (despesa.descricao != '') {
            console.log('filto de descricao')
            despesasFiltradas = despesasFiltradas.filter(d => d.descricao == despesa.descricao)
        }
        if (despesa.valor != '') {
            console.log('filto de valor')
            despesasFiltradas = despesasFiltradas.filter(d => d.valor == despesa.valor)
        }
        console.log(despesasFiltradas)
        return despesasFiltradas
    }
    remover(id) {
        localStorage.removeItem(id)
    }
}
let bd = new Bd()
function cadastrarDespesa() {
    let ano = document.getElementById('ano')
    let mes = document.getElementById('mes')
    let dia = document.getElementById('dia')
    let tipo = document.getElementById('tipo')
    let descricao = document.getElementById('descricao')
    let valor = document.getElementById('valor')

    let despesa = new Despesa(
        ano.value,
        mes.value,
        dia.value,
        tipo.value,
        descricao.value,
        valor.value
    )
    if (despesa.validarDados()) {
        bd.gravar(despesa)
        document.getElementById('modal_titulo').innerHTML = 'Registro inserido com sucesso'
        document.getElementById('modal_titulo_div').className = 'modal-header text-success'
        document.getElementById('modal_conteudo').innerHTML = 'Despesa registrada com sucesso'
        document.getElementById('modal_btn').innerHTML = 'Voltar'
        document.getElementById('modal_btn').className = 'btn btn-success'
        //dialog de sucesso
        $('#modalRegistraDespesa').modal('show')
        //limpar campos
        ano.value = ''
        mes.value = ''
        dia.value = ''
        tipo.value = ''
        descricao.value = ''
        valor.value = ''
    } else {
        //dialog de erro
        document.getElementById('modal_titulo').innerHTML = 'Erro na inclusão do registro'
        document.getElementById('modal_titulo_div').className = 'modal-header text-danger'
        document.getElementById('modal_conteudo').innerHTML = 'Preencha todos os campos'
        document.getElementById('modal_btn').innerHTML = 'Voltar e corrigir'
        document.getElementById('modal_btn').className = 'btn btn-danger'
        $('#modalRegistraDespesa').modal('show')
    }
}
function carregarListaDespesas(despesas = Array(), filtro = false) {
    if (despesas.length == 0 && filtro == false) despesas = bd.recuperarTodosRegistros()

    let listaDespesas = document.getElementById('listaDespesas')
    listaDespesas.innerHTML = ''

    // percorrer o array despesas, listando cada despesa de forma dinâmica
    despesas.forEach(function (d) {
        let linha = listaDespesas.insertRow()
        linha.insertCell(0).innerHTML = `${d.dia}/${d.mes}/${d.ano}`
        switch (d.tipo) {
            case '1': d.tipo = 'Alimentação'
                break
            case '2': d.tipo = 'Educação'
                break
            case '3': d.tipo = 'Lazer'
                break
            case '4': d.tipo = 'Saúde'
                break
            case '5': d.tipo = 'Transporte'
                break
        }
        linha.insertCell(1).innerHTML = d.tipo
        linha.insertCell(2).innerHTML = d.descricao
        linha.insertCell(3).innerHTML = d.valor

        let btn = document.createElement('button')
        btn.className = 'btn btn-danger'
        btn.innerHTML = '<i class="fas fa-times"></i>'
        btn.id = `id_despesa_${d.id}`
        btn.onclick = function () {
            let id = this.id.replace('id_despesa_', '')
            //alert(id) // para testes de id da despesa
            bd.remover(id)// remover o registro do banco de dados
            window.location.reload()// recarregar a tela de forma programática
        }
        linha.insertCell(4).append(btn)
        console.log(d)
    })
}
function pesquisarDespesa() {
    let ano = document.getElementById('ano')
    let mes = document.getElementById('mes')
    let dia = document.getElementById('dia')
    let tipo = document.getElementById('tipo')
    let descricao = document.getElementById('descricao')
    let valor = document.getElementById('valor')
    let despesa = new Despesa(ano.value, mes.value, dia.value, tipo.value, descricao.value, valor.value)
    let despesas = bd.pesquisar(despesa)
    
    this.carregarListaDespesas(despesas, true)
}