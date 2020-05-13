import React from 'react'
import ItemsTable from './ItemsTable';
import MyTown from './MyTown';
import { Container, Image, Tab } from 'semantic-ui-react'
import './App.css';
import logo from './logo.PNG';


const App = props =>{


  const GetPanes = () => {
    return [
    {
      menuItem: 'Prices',
      render: () => <Tab.Pane attached={false}><ItemsTable/></Tab.Pane>,
    },
    {
      menuItem: 'My Town',
      render: () => <Tab.Pane attached={false}><MyTown/></Tab.Pane>,
    }]
  }
  return (
    <Container className="container">
      <Image className="logo" src={logo} />
      <Tab menu={{ pointing: true }} panes={GetPanes()} />
    </Container>
  );
}

export default App;
