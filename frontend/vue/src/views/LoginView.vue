<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import type { FormInstance, FormRules } from 'element-plus'
import { User, Lock, Key } from '@element-plus/icons-vue'
import { useAuthStore } from '@/stores/auth'
import type { LoginPayload } from '@/api/auth'

const route = useRoute()
const router = useRouter()
const auth = useAuthStore()

const formRef = ref<FormInstance>()
const form = reactive<LoginPayload>({
  usernameOrEmail: '',
  password: ''
})

const rules: FormRules<LoginPayload> = {
  usernameOrEmail: [
    { required: true, message: 'Please enter your username or email', trigger: 'blur' },
    { min: 2, message: 'Must be at least 2 characters', trigger: 'blur' }
  ],
  password: [
    { required: true, message: 'Please enter your password', trigger: 'blur' },
    { min: 1, message: 'Please enter your password', trigger: 'blur' }
  ]
}

async function handleSubmit() {
  if (!formRef.value) return
  const valid = await formRef.value.validate().catch(() => false)
  if (!valid) return

  try {
    await auth.login(form)
    ElMessage.success(`Welcome back, ${auth.displayName}`)
    const redirect = route.query.redirect as string | undefined
    router.push(redirect && redirect.startsWith('/') ? redirect : { name: 'home' })
  } catch (err) {
    ElMessage.error(err instanceof Error ? err.message : 'Login failed')
  }
}
</script>

<template>
  <div class="login-page">
    <!-- Brand panel -->
    <aside class="login-brand">
      <div class="brand-inner">
        <div class="brand-logo">
          <el-icon :size="30"><Key /></el-icon>
          <span>DevForge</span>
        </div>
        <h1 class="brand-title">Build, track, ship.</h1>
        <p class="brand-subtitle">
          An internal platform to manage requirements, applications, and developers — with full
          traceability in one place.
        </p>
        <ul class="brand-features">
          <li>Trace requirements to the apps that deliver them</li>
          <li>Keep developer ownership clear</li>
          <li>Stay aligned from idea to release</li>
        </ul>
      </div>
    </aside>

    <!-- Form panel -->
    <main class="login-form-panel">
      <div class="login-card">
        <h2 class="login-title">Sign in</h2>
        <p class="login-muted">Use your DevForge account to continue.</p>

        <el-form
          ref="formRef"
          :model="form"
          :rules="rules"
          label-position="top"
          size="large"
          @keyup.enter="handleSubmit"
        >
          <el-form-item label="Username or email" prop="usernameOrEmail">
            <el-input
              v-model="form.usernameOrEmail"
              placeholder="username or email"
              :prefix-icon="User"
              autocomplete="off"
            />
          </el-form-item>

          <el-form-item label="Password" prop="password">
            <el-input
              v-model="form.password"
              type="password"
              placeholder="••••••"
              :prefix-icon="Lock"
              show-password
              autocomplete="new-password"
            />
          </el-form-item>

          <el-button
            type="primary"
            class="login-submit"
            :loading="auth.loading"
            @click="handleSubmit"
          >
            Sign in
          </el-button>
        </el-form>

        <div class="login-foot">
          Don't have an account?<RouterLink to="/register" class="login-link">Create one</RouterLink>
        </div>
      </div>
    </main>
  </div>
</template>

<style scoped>
.login-page {
  min-height: 100vh;
  display: grid;
  grid-template-columns: 1.1fr 1fr;
}

.login-brand {
  background: radial-gradient(120% 120% at 15% 0%, #22315c 0%, var(--df-bg) 65%);
  display: flex;
  align-items: center;
  padding: 64px;
}

.brand-inner {
  max-width: 480px;
}

.brand-logo {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 22px;
  font-weight: 700;
  color: var(--df-text);
  letter-spacing: 0.5px;
}

.brand-logo .el-icon {
  display: grid;
  place-items: center;
  width: 44px;
  height: 44px;
  border-radius: 12px;
  background: linear-gradient(135deg, var(--df-primary), #8a63f5);
  color: #fff;
}

.brand-title {
  margin: 40px 0 16px;
  font-size: 42px;
  line-height: 1.15;
  font-weight: 700;
}

.brand-subtitle {
  margin: 0 0 28px;
  font-size: 17px;
  line-height: 1.7;
  color: var(--df-text-muted);
}

.brand-features {
  margin: 0;
  padding: 0;
  list-style: none;
  display: grid;
  gap: 12px;
}

.brand-features li {
  position: relative;
  padding-left: 24px;
  color: var(--df-text);
  font-size: 15px;
}

.brand-features li::before {
  content: '';
  position: absolute;
  left: 0;
  top: 8px;
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: var(--df-success);
}

.login-form-panel {
  display: grid;
  place-items: center;
  padding: 48px;
}

.login-card {
  width: 100%;
  max-width: 380px;
}

.login-title {
  margin: 0 0 6px;
  font-size: 28px;
  font-weight: 700;
}

.login-muted {
  margin: 0 0 28px;
  color: var(--df-text-muted);
}

.login-submit {
  width: 100%;
  margin-top: 8px;
  font-weight: 600;
  letter-spacing: 0.3px;
}

.login-hint {
  margin-top: 20px;
  text-align: center;
}

.login-foot {
  margin-top: 20px;
  text-align: center;
  color: var(--df-text-muted);
  font-size: 14px;
}

.login-link {
  margin-left: 6px;
  font-weight: 600;
  color: var(--df-primary);
}

/* Dark-mode overrides for Element Plus form fields */
:deep(.el-input__wrapper) {
  background: var(--df-bg-soft);
  box-shadow: 0 0 0 1px var(--df-border) inset;
  border-radius: 8px;
}
:deep(.el-input__inner) {
  color: var(--df-text);
  caret-color: var(--df-text);
}
:deep(.el-form-item__label) {
  color: var(--df-text);
}

/* Browser autofill hard-codes a white background; neutralize it. */
:deep(.el-input__inner:-webkit-autofill),
:deep(.el-input__inner:-webkit-autofill:hover),
:deep(.el-input__inner:-webkit-autofill:focus) {
  -webkit-text-fill-color: var(--df-text);
  -webkit-box-shadow: 0 0 0 1000px var(--df-bg-soft) inset;
  caret-color: var(--df-text);
  background-color: transparent;
  transition: background-color 9999s ease-in-out 0s;
}

@media (max-width: 900px) {
  .login-page {
    grid-template-columns: 1fr;
  }
  .login-brand {
    display: none;
  }
}
</style>