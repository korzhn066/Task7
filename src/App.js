import logo from './logo.svg';
import './App.css';

import { ChatItem } from 'react-chat-elements'

function App() {
  return (
    <div className="App">
      <ChatItem
        avatar={'https://facebook.github.io/react/img/logo.svg'}
        alt={'Reactjs'}
        title={'Facebook'}
        subtitle={'What are you doing?'}
        date={new Date()}
        unread={0}
      />
    </div>
  );
}

export default App;
