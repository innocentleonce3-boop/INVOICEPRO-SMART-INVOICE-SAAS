import {
  ArrowUpRight,
  BadgeDollarSign,
  Bell,
  Building2,
  FileText,
  LayoutDashboard,
  PackageSearch,
  Plus,
  ReceiptText,
  Search,
  TrendingUp,
  Users,
} from 'lucide-react'
import {
  Bar,
  BarChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts'
import { BrowserRouter, NavLink, Route, Routes } from 'react-router-dom'

const navItems = [
  { to: '/', label: 'Dashboard', icon: LayoutDashboard },
  { to: '/invoices', label: 'Invoices', icon: FileText },
  { to: '/customers', label: 'Customers', icon: Users },
  { to: '/products', label: 'Products', icon: PackageSearch },
  { to: '/reports', label: 'Reports', icon: TrendingUp },
]

const statCards = [
  { label: 'Total invoiced', value: '$128.4K', trend: '+12.4%', tone: 'navy' },
  { label: 'Amount paid', value: '$94.2K', trend: '+8.2%', tone: 'green' },
  { label: 'Outstanding', value: '$34.2K', trend: '-4.3%', tone: 'amber' },
  { label: 'Overdue', value: '$14.6K', trend: '+2.1%', tone: 'red' },
]

const revenueData = [
  { month: 'Jan', revenue: 12 },
  { month: 'Feb', revenue: 18 },
  { month: 'Mar', revenue: 16 },
  { month: 'Apr', revenue: 21 },
  { month: 'May', revenue: 24 },
  { month: 'Jun', revenue: 19 },
  { month: 'Jul', revenue: 28 },
]

const recentInvoices = [
  { number: 'INV-1008', customer: 'Northwind Labs', total: '$1,240.00', status: 'Sent' },
  { number: 'INV-1007', customer: 'Aster & Co.', total: '$3,620.00', status: 'Paid' },
  { number: 'INV-1006', customer: 'Blue Peak', total: '$980.00', status: 'Draft' },
  { number: 'INV-1005', customer: 'Brightline Studio', total: '$2,150.00', status: 'Overdue' },
]

const topCustomers = [
  { name: 'Northwind Labs', amount: '$18,460' },
  { name: 'Aster & Co.', amount: '$16,240' },
  { name: 'Brightline Studio', amount: '$12,800' },
]

const templateTiles = [
  { label: 'Modern Professional', accent: 'blue' },
  { label: 'Minimal', accent: 'slate' },
  { label: 'Corporate', accent: 'navy' },
  { label: 'Creative', accent: 'indigo' },
]

function App() {
  return (
    <BrowserRouter>
      <div className="app-shell">
        <aside className="sidebar">
          <div className="brand-block">
            <div className="brand-mark">IP</div>
            <div>
              <strong>InvoicePro</strong>
              <span>Workspace</span>
            </div>
          </div>

          <nav className="nav-stack" aria-label="Main navigation">
            {navItems.map(({ to, label, icon: Icon }) => (
              <NavLink key={to} to={to} className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
                <Icon size={18} />
                <span>{label}</span>
              </NavLink>
            ))}
          </nav>

          <div className="sidebar-card">
            <div className="card-eyebrow">Current plan</div>
            <h3>Professional</h3>
            <p>12 invoices remaining this cycle.</p>
          </div>
        </aside>

        <main className="workspace">
          <header className="topbar">
            <div className="search-box">
              <Search size={16} />
              <input type="text" placeholder="Search invoices, customers, products" aria-label="Search" />
            </div>
            <div className="topbar-actions">
              <button className="action-button ghost" type="button">
                <Bell size={16} />
              </button>
              <button className="action-button primary" type="button">
                <Plus size={16} />
                New invoice
              </button>
            </div>
          </header>

          <Routes>
            <Route
              path="/"
              element={
                <div className="page-stack">
                  <section className="hero-panel">
                    <div>
                      <p className="eyebrow">Financial overview</p>
                      <h1>Business performance</h1>
                    </div>
                    <button className="action-button primary" type="button">
                      <ReceiptText size={16} />
                      Create invoice
                    </button>
                  </section>

                  <section className="stats-grid">
                    {statCards.map((card) => (
                      <article className={`stat-card ${card.tone}`} key={card.label}>
                        <div className="stat-topline">
                          <span>{card.label}</span>
                          <ArrowUpRight size={14} />
                        </div>
                        <div className="stat-value">{card.value}</div>
                        <div className="stat-trend">{card.trend}</div>
                      </article>
                    ))}
                  </section>

                  <section className="main-grid">
                    <article className="panel chart-panel">
                      <div className="panel-header">
                        <div>
                          <p className="eyebrow">Revenue</p>
                          <h2>Monthly revenue</h2>
                        </div>
                        <span className="badge success">+24.8%</span>
                      </div>

                      <div className="chart-wrap">
                        <ResponsiveContainer width="100%" height="100%">
                          <BarChart data={revenueData}>
                            <CartesianGrid strokeDasharray="3 3" vertical={false} />
                            <XAxis dataKey="month" stroke="#8b93a7" />
                            <YAxis stroke="#8b93a7" />
                            <Tooltip />
                            <Bar dataKey="revenue" fill="#3b82f6" radius={[8, 8, 0, 0]} />
                          </BarChart>
                        </ResponsiveContainer>
                      </div>
                    </article>

                    <article className="panel">
                      <div className="panel-header">
                        <div>
                          <p className="eyebrow">Collections</p>
                          <h2>Top customers</h2>
                        </div>
                      </div>
                      <ul className="list-rows">
                        {topCustomers.map((customer) => (
                          <li key={customer.name}>
                            <div>
                              <span className="list-title">{customer.name}</span>
                              <small>Quarter-to-date</small>
                            </div>
                            <strong>{customer.amount}</strong>
                          </li>
                        ))}
                      </ul>
                    </article>
                  </section>

                  <section className="bottom-grid">
                    <article className="panel">
                      <div className="panel-header">
                        <div>
                          <p className="eyebrow">Invoices</p>
                          <h2>Recent activity</h2>
                        </div>
                        <Link to="/invoices" className="link-button">View all</Link>
                      </div>

                      <div className="table-wrap">
                        <table>
                          <thead>
                            <tr>
                              <th>Invoice</th>
                              <th>Customer</th>
                              <th>Total</th>
                              <th>Status</th>
                            </tr>
                          </thead>
                          <tbody>
                            {recentInvoices.map((invoice) => (
                              <tr key={invoice.number}>
                                <td>{invoice.number}</td>
                                <td>{invoice.customer}</td>
                                <td>{invoice.total}</td>
                                <td>
                                  <span className={`status-pill ${invoice.status.toLowerCase()}`}>{invoice.status}</span>
                                </td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    </article>

                    <article className="panel">
                      <div className="panel-header">
                        <div>
                          <p className="eyebrow">Templates</p>
                          <h2>Brand-ready layouts</h2>
                        </div>
                      </div>
                      <div className="template-grid">
                        {templateTiles.map((template) => (
                          <div key={template.label} className={`template-card ${template.accent}`}>
                            <Building2 size={18} />
                            <span>{template.label}</span>
                          </div>
                        ))}
                      </div>
                    </article>
                  </section>
                </div>
              }
            />

            <Route path="/invoices" element={<PlaceholderPage title="Invoices" subtitle="Invoice creation, dispatch, and payment tracking live here." />} />
            <Route path="/customers" element={<PlaceholderPage title="Customers" subtitle="Customer records, balances, and activity are managed in this workspace." />} />
            <Route path="/products" element={<PlaceholderPage title="Products & Services" subtitle="Catalog items and default pricing are configured here." />} />
            <Route path="/reports" element={<PlaceholderPage title="Reports & Analytics" subtitle="Revenue, aging, and operational analysis are surfaced here." />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  )
}

function PlaceholderPage({ title, subtitle }: { title: string; subtitle: string }) {
  return (
    <div className="page-stack">
      <section className="panel empty-state">
        <div className="empty-icon">
          <BadgeDollarSign size={22} />
        </div>
        <h2>{title}</h2>
        <p>{subtitle}</p>
      </section>
    </div>
  )
}

export default App
