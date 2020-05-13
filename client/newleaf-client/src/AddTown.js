import React, { useState, useEffect } from 'react';
import { Select, Input, Grid, Button, Label } from 'semantic-ui-react'
import SemanticDatepicker from 'react-semantic-ui-datepickers';
import 'react-semantic-ui-datepickers/dist/react-semantic-ui-datepickers.css';


const AddTown = props => {
    const { fruits, userName, addNewTown } = props;
    const [selectedFruit, setSelectedFruit] = useState(null);
    const [townName, setTownName] = useState('');
    const [mayorName, setMayorName] = useState('');
    const [createdDate, setCreatedDate] = useState(new Date());
    const onChange = (event, data) => setCreatedDate(data.value);

    const getFruitOptions = () => {
        let fruitOptions = [];
        for (let i = 0; i < fruits.length; i++) {
            fruitOptions.push({ key: i, value: i, text: fruits[i] });
        }
        return fruitOptions;
    }
    const AddTown = () => {
        fetch(`https://acnlapi.azurewebsites.net/api/town/add?userName=${userName}&townName=${townName}&mayorName=${mayorName}&createdDate=${createdDate.toISOString()}&nativeFruit=${selectedFruit}`)
            .then(response => response.json())
            .then((data) => {
                addNewTown([data]);
            }
            );

    }
    return (
        <Grid stackable columns={2}>
            <Grid.Row>
                <Grid.Column width={8}>
                    <span>A Town wasn't found for your account, please add one!</span>
                </Grid.Column>
            </Grid.Row>
            <Grid.Row>
                <Grid.Column>
                    <Label size="large">Town Name</Label>
                </Grid.Column>
                <Grid.Column>
                    <Input
                        onChange={(e) => setTownName(e.target.value)}
                    />
                </Grid.Column>
            </Grid.Row>
            <Grid.Row>
                <Grid.Column>
                    <Label size="large">Mayor Name</Label>
                </Grid.Column>
                <Grid.Column>
                    <Input
                        onChange={(e) => setMayorName(e.target.value)}
                    />
                </Grid.Column>
            </Grid.Row>
            <Grid.Row>
                <Grid.Column>
                    <Label size="large">Created Date</Label>
                </Grid.Column>
                <Grid.Column>
                    <SemanticDatepicker
                        onChange={onChange} />
                </Grid.Column>
            </Grid.Row>
            <Grid.Row>
                <Grid.Column>
                    <Label size="large">Native Fruit</Label>
                </Grid.Column>

                <Grid.Column>
                    <Select
                        placeholder='Select your fruit'
                        options={getFruitOptions()}
                        onChange={(e, v) => setSelectedFruit(v.value)}
                    />
                </Grid.Column>
            </Grid.Row>
            <Grid.Row>
                <Grid.Column>
                    <Button onClick={AddTown} primary>Save</Button>
                </Grid.Column>
            </Grid.Row>
        </Grid>
    );
}
export default AddTown;