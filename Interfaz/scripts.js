
const addExpensebtton= document.getElementById('add_expense');
const deletebton= document.getElementById('deletebton');
const editbton= document.getElementById('editbton');
const consolidatebton= document.getElementById('consolidatebton');


const addIncomebtton= document.getElementById('add_income');
const IncomeAmount_input= document.getElementById('IncomeAmount')

const description_input= document.getElementById('description');
const amount_input= document.getElementById('amount');
const label_input= document.getElementById('label');
const expenses_container= document.getElementById('expenses_container');
const currentMoney= document.getElementById('currentMoney')
const consolidatedMoney= document.getElementById('consolidatedMoney')
let istsconsolidated=0



const montoingreso_input= document.getElementById('montoingreso');

function GetCurrentMoney(id)
{
    fetch(`https://localhost:7256/api/accounts/getCurrentMoney/${id}`,{
        method : 'GET',
        headers:{
            "content-type": "application/json"
        }
    
        })
        .then(data => data.json())
        .then(response=> showMoney(response))
}
function GetConsolidatedMoney(id)
{
    fetch(`https://localhost:7256/api/Accounts/getConsolidatedMoney/${id}`,{
        method : 'GET',
        headers:{
            "content-type": "application/json"
        }
    
        })
        .then(data => data.json())
        .then(response=> showConsolidatedMoney(response))
}

function showMoney(money)
{
   
    cm=`
        <p>Current Money:</p>
        <p><strong>${money}</strong></p> 
    `
    currentMoney.innerHTML=cm
}
function showConsolidatedMoney(money)
{
    
    cm=`
        <p>Consolidated Money:</p>
        <p><strong>${money}</strong></p> 
    `
    consolidatedMoney.innerHTML=cm
}



function clearForm(){
    description_input.value='';
    amount_input.value='';
    label_input.value='';
}


function Actions(expense_item)
{
    deletebton.setAttribute('data-id',expense_item.dataset.id)
    deletebton.classList.remove('hidden');
    editbton.setAttribute('data-id',expense_item.dataset.id)
    editbton.classList.remove('hidden');
    consolidatebton.setAttribute('data-id',expense_item.dataset.id)
    consolidatebton.classList.remove('hidden');
}


function AddExpense(description,amount,label)
{
    const body={
        
        cant: amount,
        description : description,
        label: label
        
    };
    fetch(`https://localhost:7256/api/expenses/${amount}/${description}/${label}`,{
    method : 'POST',
   // body : JSON.stringify(body),
    headers:{
        "content-type": "application/json"
    }

    })
    .then(data => data.json)
    .then(response =>{
        clearForm();
        GetAllExpenses();
        
    })
    
    
}


function displayExpenses(egresos)
{
    let allEgreso='';

    egresos.forEach(egreso => {
        const egresoElement=`
                        <div class="egreso_item" data-id="${egreso.id}" >
                            <p>Description:${egreso.description}</p>
                            <p>Date:${egreso.date}</p>
                            <p>Amount:$${egreso.amount}</p>
                            <p>Label:${egreso.label}</p>
                            <p>Consolidated:${egreso.consolidated}</p>
                        </div>
                        `;
        allEgreso+=egresoElement
    });
    expenses_container.innerHTML=allEgreso;


    document.querySelectorAll('.egreso_item').forEach(egreso_item =>{
        egreso_item.addEventListener('click',function(){
            
            Actions(egreso_item)
        })
        
        })

}

function GetAllExpenses(){
    fetch('https://localhost:7256/api/expenses')
    .then(data => data.json())
    .then(response => displayExpenses(response))
}

function DeleteExpense(id)
{
    
  fetch(`https://localhost:7256/api/expenses/${id}`,{
    method: 'DELETE',
    headers:{
        "content-type": "application/json"
    }
  })
  .then(response =>{
    GetAllExpenses();
})
}

function consolidate(id)
{
    fetch(`https://localhost:7256/api/expenses/consolidate/${id}`,{
    method: 'PUT',
    headers:{
        "content-type": "application/json"
    }
  })
  .then(response =>{
    GetAllExpenses();
    GetConsolidatedMoney(`EF926559-2FB8-460D-9B42-22BA4CE934E1`)
})
  
}


function AddIncome(amount)
{
    fetch(`https://localhost:7256/api/incomes/${amount}`,{
        method : 'POST',
        headers:{
            "content-type": "application/json"
        }
        })
        .then(response =>  GetCurrentMoney(`EF926559-2FB8-460D-9B42-22BA4CE934E1`))

}

function EditExpense(id,description,amount,label)
{
   
    
    const body={
        id:id,
        amount: amount,
        description : description,
        label: label
        
    };

    fetch(`https://localhost:7256/api/expenses/${id}`,{
    method : 'PUT',
    body : JSON.stringify(body),
    headers:{
        "content-type": "application/json"
    }
    })
    .then(data => data.json)
    .then(response =>{
        clearForm();
        GetAllExpenses();
        
    })



}

function istsconso(expense)
{
istsconsolidated=expense.consolidated


}


function GetExpenseById(id)
{
    fetch(`https://localhost:7256/api/expenses/getExpenseById/${id}`,{
        method: 'GET',
        headers:{
            "content-type": "application/json"
        }
      })
     .then(data=> data.json())

     .then(response => istsconso(response))    
}

addExpensebtton.addEventListener('click',function()
{
    AddExpense(description_input.value,amount_input.value,label_input.value)
})

addIncomebtton.addEventListener('click',function()
{
    AddIncome(IncomeAmount_input.value)
   
})

deletebton.addEventListener('click',function(){
    const id = consolidatebton.dataset.id;
    DeleteExpense(id)
    editbton.classList.add('hidden');
    deletebton.classList.add('hidden');
    consolidatebton.classList.add('hidden');

})
consolidatebton.addEventListener('click',function(){
    const id = deletebton.dataset.id;
    consolidate(id);
    editbton.classList.add('hidden');
    deletebton.classList.add('hidden');
    consolidatebton.classList.add('hidden');

})

editbton.addEventListener('click',function(){
    
    const id = editbton.dataset.id;

    if (description_input.value=='')
        alert("Add description")
    
    if (amount_input.value=='')
        alert("Add monto")
  
    if (label_input.value=='')
        alert("Add etiqueta")

    else 
    {
        if(istsconsolidated==1)
        {
            alert()
        }
        EditExpense(id,description_input.value,amount_input.value,label_input.value);
        editbton.classList.add('hidden');
        deletebton.classList.add('hidden');
        consolidatebton.classList.add('hidden');
    }

})

GetAllExpenses();

GetCurrentMoney(`EF926559-2FB8-460D-9B42-22BA4CE934E1`)
GetConsolidatedMoney(`EF926559-2FB8-460D-9B42-22BA4CE934E1`)


