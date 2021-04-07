import React from 'react';
import FlipMove from "react-flip-move";
import './App.css';

function App() {
	return (
		<div className="App">
			<section className="hero is-small is-danger">
				<div className="hero-body">
					<h1 className="title">Mock.me</h1>
					<h2 className="subtitle">For mocking all of your APIs</h2>
				</div>
			</section>
			<Content />
		</div>
	);
}

class Content extends React.Component {
	render() {
		return (
			<section className="section is-small">
				<div className="tabs">
					<ul>
						<li className="is-active">
							<a>
								<span className="icon is-small"><i className="fas fa-code" aria-hidden="true"></i></span>
								<span>Configure Mocks</span>
							</a>
						</li>
						<li>
							<a>
								<span className="icon is-small"><i className="fas fa-file-alt" aria-hidden="true"></i></span>
								<span>View Request Logs</span>
							</a>
						</li>
					</ul>
				</div>
				<Config />
			</section>
		);
	}
}

class Config extends React.Component {
	constructor(props) {
		super(props);
		this.handleCreate = this.handleCreate.bind(this);
		this.handleUpdate = this.handleUpdate.bind(this);
		this.handleDelete = this.handleDelete.bind(this);
		this.handleAddNewMockClick = this.handleAddNewMockClick.bind(this);
		this.handleCancelNewMockClick = this.handleCancelNewMockClick.bind(this);
		this.state = {
			showNewMockInput: false,
			isLoaded: false,
			mocks: [],
			error: null,
			newMockError: null
		};
	}
	
	componentDidMount() {
		fetch("https://mockapiservice.azurewebsites.net/MockConfig")
			.then(res => res.json())
			.then(
				(result) => {
					this.setState({
						isLoaded: true,
						mocks: result
					});
				},
				(error) => {
					this.setState({
						isLoaded: true,
						error
					});
				}
			)
	}
	
	handleCreate(newMock) {		
		const request = {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(newMock)
		}
			
		fetch("https://mockapiservice.azurewebsites.net/MockConfig", request)
			.then(res => res.json())
			.then(
				(newMockReference) => {
					fetch("https://mockapiservice.azurewebsites.net/MockConfig/" + newMockReference)
						.then(res => res.json())
						.then(
							(newMock) => {
								this.setState({
									mocks: [newMock].concat(this.state.mocks),
									showNewMockInput: false
								})
							},
							(error) => {
								this.setState({
									newMockError: error
								});
							}
						);
				},
				(error) => {
					this.setState({
						newMockError: error
					});
				}
			);
	}
	
	handleUpdate(mock) {
		const request = {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json'
			},
			body: mock
		}
			
		fetch("https://mockapiservice.azurewebsites.net/MockConfig/" + mock.mockReference, request)
			.then(res => res.json())
			.then(
				(result) => {
					// todo
				},
				(error) => {
					this.setState({
						error
					});
				}
			);
	}
	
	
	handleDelete(mockReference) {
		const request = {
			method: 'DELETE'
		}
			
		fetch("https://mockapiservice.azurewebsites.net/MockConfig/" + mockReference, request)
			.then(
				(result) => {
					var filteredMocks = this.state.mocks.filter(function (mock) {
						return (mock.reference !== mockReference);
					});

					this.setState({
						mocks: filteredMocks
					});
				},
				(error) => {
					this.setState({
						error
					});
				}
			);
	}
	
	handleAddNewMockClick() {
		this.setState({showNewMockInput: true});
	}
	
	handleCancelNewMockClick() {
		this.setState({showNewMockInput: false});
	}
	
	render() {
		const { showNewMockInput, isLoaded, mocks, error } = this.state;
		
		return (
			<React.Fragment>
				<div className="level">
					<div className="level-left">
						<div className="level-item">
							<p>
								Mock endpoints available at <strong>http://mpp-esuite-rest.mock.me</strong>
							</p>
						</div>
					</div>

					<div className="level-right">
						<p className="level-item"><a>Import Config</a></p>
						<p className="level-item"><button className="button is-link" onClick={this.handleAddNewMockClick} disabled={showNewMockInput}>Add New Mock</button></p>
					</div>
				</div>
				<NewMock visible={showNewMockInput} onCancelClick={this.handleCancelNewMockClick} handleMockSave={this.handleCreate} error={this.state.newMockError} />
				<Mocks isLoaded={isLoaded} items={mocks} error={error} handleMockDelete={this.handleDelete} />		
			</React.Fragment>
		);
	}
}

class Mocks extends React.Component {	
	render() {
		const { error, isLoaded, items } = this.props;
		if (!isLoaded) {
			return <div className="notification"><h3>Loading mocks...</h3><progress className="progress is-large is-info" /></div>;
		} else if (error) {
			return <div className="notification is-warning">Something went wrong - couldn't load the list of mocks</div>;
		} else if (items.length === 0) {
			return <div className="notification is-info">You have no mocks configured yet</div>;
		} else {
			return (
				<FlipMove duration={250} easing="ease-out">
					{items.map(item => (
						<Mock key={item.reference} mock={item} onDeleteClick={this.props.handleMockDelete} />
					))}
				</FlipMove>
			);
		}
	}
}

class NewMock extends React.Component {
	constructor(props) {
		super(props)
		this.handleEndpointInput = this.handleEndpointInput.bind(this);
		this.handleResponseHeadersInput = this.handleResponseHeadersInput.bind(this);
		this.handleResponseBodyInput = this.handleResponseBodyInput.bind(this);
		this.handleSaveClick = this.handleSaveClick.bind(this);
		this.state = {
			mock: {
				request: { method: 'POST', endpoint: '' },
				response: { headers: '', body: '' }
			}
		};
	}
	
	handleEndpointInput(e) {
		this.setState(prevState => ({
			mock: {
				...prevState.mock,
				request: {
					...prevState.mock.request,
					endpoint: e.target.value
				}
			}
		}));
	}
	
	handleResponseHeadersInput(e) {
		this.setState(prevState => ({
			mock: {
				...prevState.mock,
				response: {
					...prevState.mock.response,
					headers: e.target.value
				}
			}
		}));
	}
	
	handleResponseBodyInput(e) {
		this.setState(prevState => ({
			mock: {
				...prevState.mock,
				response: {
					...prevState.mock.response,
					body: e.target.value
				}
			}
		}));
	}
	
	handleSaveClick() {
		this.props.handleMockSave(this.state.mock);
	}
	
	render() {
		if (!this.props.visible) return (null);
		
		return (
			<div className="card mock">
				<header className="card-header">
					<p className="card-header-icon new-mock">New Mock</p>
					<div className="card-header-title">
						<div className="select">
							<select>
								<option>GET</option>
								<option>POST</option>
							</select>
						</div>
						<input className="input" type="text" placeholder="Endpoint, e.g. /test/123" onChange={this.handleEndpointInput} value={this.state.endpointValue} />
					</div>
				</header>
				<MockConfig
					responseHeaders=""
					responseBody=""
					onResponseHeadersChange={this.handleResponseHeadersInput}
					onResponseBodyChange={this.handleResponseBodyInput}
					isVisible="true"
					onCancelClick={this.props.onCancelClick}
					onSaveClick={this.handleSaveClick}
					error={this.props.error} />
			</div>
		);
	}
}

class Mock extends React.Component {
	constructor(props) {
		super(props)
		this.handleShowConfigClick = this.handleShowConfigClick.bind(this);
		this.handleHideConfigClick = this.handleHideConfigClick.bind(this);
		this.handleDeleteClick = this.handleDeleteClick.bind(this);
		this.handleChange = this.handleChange.bind(this);
		this.state = {
			mock: this.props.mock,
			showConfig: false,
			saveDisabled: true};
	}
	
	handleShowConfigClick() {
		this.setState({showConfig: true});
	}

	handleHideConfigClick() {
		this.setState({showConfig: false});
	}
	
	handleDeleteClick() {
		this.props.onDeleteClick(this.state.mock.reference);
	}
	
	handleChange() {
		this.setState({saveDisabled: false});
	}
	
	render() {
		const { mock, showConfig, saveDisabled } = this.state;
			
		return (
			<div className="card mock">
				<MockHeader
					requestMethod={mock.request.method}
					requestEndpoint={mock.request.endpoint}
					isEditButtonVisible={!showConfig}
					onEditClick={this.handleShowConfigClick}
					onDeleteClick={this.handleDeleteClick} />
				<MockConfig
					responseHeaders={mock.response.headers}
					responseBody={mock.response.body}
					isVisible={showConfig}
					saveDisabled={saveDisabled}
					onResponseHeadersChange={this.handleChange}
					onResponseBodyChange={this.handleChange}
					onCancelClick={this.handleHideConfigClick} />
			</div>
		);
	}
}

function MockHeader(props) {
	var editButton;
	if (props.isEditButtonVisible) editButton = <a className="card-header-icon" onClick={props.onEditClick}>Edit</a>
	
	return (
		<header className="card-header">
			<p className="card-header-icon method">{props.requestMethod}</p>
			<p className="card-header-title">{props.requestEndpoint}</p>
			{editButton}
			<a className="card-header-icon" onClick={props.onDeleteClick}>Delete</a>
		</header>
	);
}

function MockConfig(props) {
	if (!props.isVisible) return (null);
	
	const headersPlaceholder = "The response headers that you want this mock to return.\n\nThe following header will be used by default:\n\n{\n\t\"Content-Type\": \"application/json\"\n}";
	const bodyPlaceholder = "The response body that you want this mock to return";
	
	return (
		<div className="card-content">
			<div className="columns">
				<div className="column">
					<h3>Response Headers</h3>
					<textarea className="textarea" placeholder={headersPlaceholder} rows="10" onChange={props.onResponseHeadersChange} defaultValue={props.responseHeaders} />
				</div>
				<div className="column">
					<h3>Response Body</h3>
					<textarea className="textarea" placeholder={bodyPlaceholder} rows="10" onChange={props.onResponseBodyChange} defaultValue={props.responseBody} />
				</div>
			</div>
			
			<Warning message={props.error} />
			
			<div className="buttons is-right">
				<SubmitOrCancel submitText="Save" submitDisabled={props.saveDisabled} onSubmit={props.onSaveClick} onCancel={props.onCancelClick} />
			</div>
		</div>
	);
}

function SubmitOrCancel(props) {
	if (!props.isVisible) return (null);
	
	return (		
		<React.Fragment>
			<button className="button is-white" onClick={props.onCancel}>Cancel</button>
			<button className="button is-link" onClick={props.onSubmit} disabled={props.submitDisabled}>{props.submitText}</button>
		</React.Fragment>
	);
}

SubmitOrCancel.defaultProps = {
    isVisible: true
}

function Warning(props) {
	if (!props.message) return (null);
	
	return (
		<div className="notification is-warning">Something went wrong - couldn't save the changes, please try again</div>
	);
}

export default App;
