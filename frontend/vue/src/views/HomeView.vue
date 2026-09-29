<script setup lang="ts">
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { Key, User, SwitchButton } from '@element-plus/icons-vue'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()

const stats = [
  { label: 'Requirements', value: 0, icon: 'Files' },
  { label: 'Applications', value: 0, icon: 'Grid' },
  { label: 'Developers', value: 0, icon: 'User' }
]

async function handleLogout() {
  await auth.logout()
  ElMessage.success('Signed out')
  router.push({ name: 'login' })
}
</script>

<template>
  <div class="home">
    <!-- Top bar -->
    <header class="topbar">
      <div class="topbar-brand">
        <el-icon :size="20"><Key /></el-icon>
        <span>DevForge</span>
      </div>
      <div class="topbar-right">
        <div class="topbar-user">
          <span class="avatar">{{ auth.displayName.charAt(0).toUpperCase() }}</span>
          <div class="user-meta">
            <span class="user-name">{{ auth.user?.fullName || auth.user?.username }}</span>
            <span class="user-role">{{ auth.user?.role }}</span>
          </div>
        </div>
        <el-button text :icon="SwitchButton" class="logout-btn" @click="handleLogout">
          Sign out
        </el-button>
      </div>
    </header>

    <!-- Sidebar -->
    <div class="body">
      <aside class="sidebar">
        <nav class="nav">
          <span class="nav-item active">
            <el-icon><User /></el-icon> Overview
          </span>
          <span class="nav-item">
            <el-icon><Files /></el-icon> Requirements
          </span>
          <span class="nav-item">
            <el-icon><Grid /></el-icon> Applications
          </span>
          <span class="nav-item">
            <el-icon><User /></el-icon> Developers
          </span>
        </nav>
      </aside>

      <!-- Content -->
      <main class="content">
        <h1 class="content-title">Overview</h1>
        <p class="content-subtitle">Welcome to DevForge, {{ auth.displayName }}. Here's where things stand.</p>

        <section class="stats-grid">
          <div v-for="s in stats" :key="s.label" class="stat-card">
            <el-icon :size="22" class="stat-icon"><component :is="s.icon" /></el-icon>
            <div class="stat-value">{{ s.value }}</div>
            <div class="stat-label">{{ s.label }}</div>
          </div>
        </section>

        <section class="panel">
          <h2 class="panel-title">Getting started</h2>
          <el-empty description="No data yet — more modules are coming soon." />
        </section>
      </main>
    </div>
  </div>
</template>

<style scoped>
.home {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* Top bar */
.topbar {
  height: 64px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 24px;
  background: var(--df-bg-soft);
  border-bottom: 1px solid var(--df-border);
}

.topbar-brand {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: 700;
  font-size: 18px;
}

.topbar-right {
  display: flex;
  align-items: center;
  gap: 16px;
}

.topbar-user {
  display: flex;
  align-items: center;
  gap: 10px;
}

.avatar {
  display: grid;
  place-items: center;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: linear-gradient(135deg, var(--df-primary), #8a63f5);
  color: #fff;
  font-weight: 600;
}

.user-meta {
  display: flex;
  flex-direction: column;
  line-height: 1.2;
}

.user-name {
  font-weight: 600;
  font-size: 14px;
}

.user-role {
  font-size: 12px;
  color: var(--df-text-muted);
}

.logout-btn {
  color: var(--df-text-muted);
}
.logout-btn:hover {
  color: var(--df-danger);
}

/* Body + sidebar */
.body {
  display: flex;
  flex: 1;
  min-height: 0;
}

.sidebar {
  width: 232px;
  padding: 20px 12px;
  border-right: 1px solid var(--df-border);
  background: var(--df-bg-soft);
}

.nav {
  display: grid;
  gap: 6px;
}

.nav-item {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 11px 14px;
  border-radius: 8px;
  cursor: pointer;
  color: var(--df-text-muted);
  font-size: 14px;
  transition: background 0.15s, color 0.15s;
}

.nav-item:hover {
  background: var(--df-bg);
  color: var(--df-text);
}

.nav-item.active {
  background: var(--df-primary);
  color: #fff;
  font-weight: 600;
}

/* Content */
.content {
  flex: 1;
  padding: 32px 40px;
  overflow-y: auto;
}

.content-title {
  margin: 0 0 6px;
  font-size: 28px;
  font-weight: 700;
}

.content-subtitle {
  margin: 0 0 28px;
  color: var(--df-text-muted);
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: 16px;
  margin-bottom: 28px;
}

.stat-card {
  padding: 22px;
  border-radius: 14px;
  border: 1px solid var(--df-border);
  background: var(--df-bg-soft);
}

.stat-icon {
  color: var(--df-primary);
  margin-bottom: 14px;
}

.stat-value {
  font-size: 32px;
  font-weight: 700;
}

.stat-label {
  margin-top: 4px;
  color: var(--df-text-muted);
  font-size: 14px;
}

.panel {
  border: 1px solid var(--df-border);
  border-radius: 14px;
  background: var(--df-bg-soft);
  padding: 24px;
}

.panel-title {
  margin: 0 0 12px;
  font-size: 18px;
  font-weight: 600;
}

@media (max-width: 720px) {
  .stats-grid {
    grid-template-columns: 1fr;
  }
  .sidebar {
    display: none;
  }
}
</style>