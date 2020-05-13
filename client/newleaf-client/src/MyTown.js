import React, { useState, useEffect } from 'react';
import Login from './Login';
import AddTown from './AddTown';


const fruits = ["Apples", "Cherries", "Oranges", "Peaches", "Pears"];

const MyTown = props => {
    const [towns, setTowns] = useState([]);
    const [userName, setUserName] = useState(null);

    useEffect(() => {
        fetch(`https://acnlapi.azurewebsites.net/api/town/get?userName=${userName}`)
        .then(response => response.json())
        .then((data) => {
            setTowns(data);
        }
        );
    }, [userName]);


    const renderTowns = () => {
        if(userName && towns && towns.length > 0) {
            return (
                <ul>
                    <li>Town {towns[0].name}</li>
                    <li>Mayor {towns[0].mayorName}</li>
                    <li>Created {towns[0].created}</li>
                    <li>Fruit {fruits[towns[0].nativeFruit]}</li>
                </ul>
            )
        }
        if(userName && (!towns || towns.length === 0)) {
            return <AddTown fruits={fruits} userName={userName} addNewTown={(towns) => setTowns(towns)}/>
        }
        return  <Login performLogin={(un) => setUserName(un)} />;
    }

    return (
        <div>
            {renderTowns()}            
        </div>
    );
}
export default MyTown;